using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace LogAppLibrary
{
    public class UserModel
    {

        public int Id { get; set; }
        public string student_ID { get; set; }
        public int age { get; set; }
        public string contact_info { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        public UserModel() {}

        public UserModel(string studentID_, string age_, string contact_info_, string first_name_, string last_name_)
        {
            /*int studentIDValue = 0;
            int.TryParse(studentID_, out studentIDValue);
            student_ID = studentIDValue;*/
            student_ID = studentID_;

            int ageValue = 0;
            int.TryParse(age_, out ageValue);
            age = ageValue;

            /*int contact_infoValue = 0;
            int.TryParse(contact_info_, out contact_infoValue);
            contact_info = contact_infoValue;*/

            contact_info = contact_info_;
            first_name = first_name_;
            last_name = last_name_;
        }

        public UserModel(string studentID) //just for when submiting the id for first entry so sql can point the time exactly when id is made
        {
            /*int studentIDValue = 0;
            int.TryParse(studentID, out studentIDValue);
            student_ID = studentIDValue; */

            student_ID = studentID;
        }
    }
}
