using Android.Provider;
using Plugin.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsingContactService.Droid;
using WeatherApp.DependencyServices;
using WeatherApp.Models;
using Xamarin.Forms;
using Plugin.Permissions.Abstractions;

[assembly: Dependency(typeof (ContactService_Droid))]
namespace UsingContactService.Droid
{
    public class ContactService_Droid : IContactService
    {
        public async Task<IEnumerable<PhoneContact>> GetAllContacts()
        {
            var contactList = new List<PhoneContact>();
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;
            string[] projection = { ContactsContract.Contacts.InterfaceConsts.Id,
            ContactsContract.Contacts.InterfaceConsts.DisplayName,
            ContactsContract.CommonDataKinds.Phone.Number };
            if (await PermissionGranting())
            {
                //await Task.Run(() => {
                var cursor = Android.App.Application.Context.ContentResolver.Query(uri, projection, null, null, null);

                if (cursor.MoveToFirst())
                {
                        do
                        {
                            var muser = new PhoneContact();
                            var phoneNumberList = new List<PhoneNumber>();

                            //var number = Regex.Replace(cursor.GetString(cursor.GetColumnIndex(projection[2])), @"[^\u0000-\u007F]+", string.Empty);
                            var number = cursor.GetString(cursor.GetColumnIndex(projection[2]));
                            //  number.Replace(" ", "").Replace("(", "").Replace(")","");
                            phoneNumberList.Add(new PhoneNumber
                            {
                                Digits = number
                            });
                            muser._id = cursor.GetString(cursor.GetColumnIndex(projection[0]));
                            //muser._id = number;
                            muser.LocalName = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                            muser.PhoneNumbers = phoneNumberList;
                            if (!string.IsNullOrEmpty(number) && !contactList.Any(x => x._id == muser._id))
                                contactList.Add(muser);
                        } while (cursor.MoveToNext());
                }
                //});
                //var OrderContactList = contactList.OrderBy(item => item.LocalName);
            }
            return contactList;

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