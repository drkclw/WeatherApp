using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBook;
using Contacts;
using Foundation;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using UIKit;
using UsingContactservice.iOS;
using WeatherApp.DependencyServices;
using WeatherApp.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContactService_iOS))]
namespace UsingContactservice.iOS
{
    public class ContactService_iOS : IContactService
    {
        public async Task<IEnumerable<PhoneContact>> GetAllContacts()
        {
            if (!(await PermissionGranting()))
                return new List<PhoneContact>();

            var keysTOFetch = new[] { CNContactKey.Identifier, CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.PhoneNumbers };
            NSError error;
            CNContact[] contactList;
            var ContainerId = new CNContactStore().DefaultContainerIdentifier;
            using (var predicate = CNContact.GetPredicateForContactsInContainer(ContainerId))

            using (var store = new CNContactStore())
            {
                contactList = store.GetUnifiedContacts(predicate, keysTOFetch, out error);
            }
            var contacts = new List<PhoneContact>();

            foreach (CNContact contact in contactList)
            {
                if (contact != null && contact.PhoneNumbers != null)
                {
                    contacts.Add(new PhoneContact
                    {
                        _id = $"{contact.Identifier}",
                        FirstName = contact.GivenName,
                        LastName = contact.FamilyName,
                        NickName = contact.Nickname,
                        PhoneNumbers = contact.PhoneNumbers.Select(pn => new PhoneNumber {
                            CountryCode = pn.Value.ValueForKey(new NSString("countryCode")).ToString(),
                            Digits = pn.Value.StringValue}
                        ).ToList()
                    });
                }
            }
            return contacts;
        }

        /// <summary>
        /// Ask the permission if not granted already.
        /// </summary>
        /// <returns>Whether the permission is granted or not.</returns>
        public async Task<bool> PermissionGranting()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);
                status = results[Permission.Contacts];
            }
            if (status != PermissionStatus.Granted)
            {
                return false;
            }

            return true;
        }
    }
}