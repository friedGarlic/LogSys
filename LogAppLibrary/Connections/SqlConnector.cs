using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LogAppLibrary
{
    public class SqlConnector : IDataConnection
    {
        public UserModel CreateUser(UserModel user_model) //for registering(creating user)
        {
            
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@StudentID", user_model.student_ID);
                p.Add("@Age", user_model.age);
                p.Add("@ContactInfo", user_model.contact_info);
                p.Add("@FirstName", user_model.first_name);
                p.Add("@LastName", user_model.last_name);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                dbConnection.Execute("dbo.spUserData",p,commandType: CommandType.StoredProcedure);
                
                user_model.Id = p.Get<int>("@id");

                return user_model;
            }
        }

        public UserModel CurrentTime(UserModel user_model) //log in, passing studentIdNumber that was on datetimetable and not colliding with other param
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@StudentIdNumber", user_model.student_ID);

                return user_model;
            }
        }
        public PurposeModel CreatePurpose(UserModel user_model, PurposeModel purpose_model) // for borrowing item and returning
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@StudentIdNumber", user_model.student_ID);
                p.Add("@ItemName", purpose_model.ItemName);
                p.Add("@TimeInOut", purpose_model.timeInOut);
                p.Add("@Quantity", purpose_model.Quantity);

                dbConnection.Execute("dbo.spPurposeData", p, commandType: CommandType.StoredProcedure);

                return purpose_model;
            }
        }
        public UserModel CurrentTime(UserModel user_model, PurposeModel purposeModel) //passing timeinout string with studentid to table
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@StudentIdNumber", user_model.student_ID);
                p.Add("@TimeInOut", purposeModel.timeInOut);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                dbConnection.Execute("dbo.spTimeData", p, commandType: CommandType.StoredProcedure);

                user_model.Id = p.Get<int>("@id");
                return user_model;
            }
        }

        public ItemModel CreateItem(ItemModel item_model) //adding/creating item to item table
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@ItemName", item_model.ItemName);
                p.Add("@Quantity", item_model.Quantity);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                dbConnection.Execute("dbo.spItemData", p, commandType: CommandType.StoredProcedure);

                item_model.Id = p.Get<int>("@id");

                return item_model;
            }
        }
        public ItemModel AddQuantityItem(ItemModel item_model)
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@ItemName", item_model.ItemName);
                p.Add("@Quantity", item_model.Quantity);

                dbConnection.Execute("dbo.spAddItemQuantity", p, commandType: CommandType.StoredProcedure);

                return item_model;
            }
        } //self description

        public ItemModel RemoveItem(ItemModel item_model) //remove quantity of item
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@ItemName", item_model.ItemName);
                p.Add("@Quantity", item_model.Quantity);

                dbConnection.Execute("dbo.spRemoveItemData", p, commandType: CommandType.StoredProcedure);

                return item_model;
            }
        }
        public ItemModel RemoveItemName(ItemModel item_model) //remove the whole column of passed itemmodel name
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@ItemName", item_model.ItemName);

                dbConnection.Execute("dbo.spRemoveItemName", p, commandType: CommandType.StoredProcedure);

                return item_model;
            }
        }

        public bool IsItemDuplicate(ItemModel item_model) //self description
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@ItemName", item_model.ItemName);

                string query = "SELECT COUNT(*) FROM Items WHERE ItemName = @ItemName";
                int count = dbConnection.ExecuteScalar<int>(query, p);

                return count > 0;
            }
        }
        public bool IsStudentIdDuplicate(UserModel user_model) // when trying to register, yet already registered
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@StudentID", user_model.student_ID);

                string query = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";
                int count = dbConnection.ExecuteScalar<int>(query, p);

                return count > 0;
            }
        }

        public void GetDuration()
        {
            using (SqlConnection connection = new SqlConnection(GlobalConfig.ConnectString("SearchCN")))
            {
                connection.Open();

                string sqlQuery = "SELECT CurDateTime FROM DateTimeTable WHERE StudentIdNumber = @StudentIdNumber";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    DateTime dateTimeValue = (DateTime)command.ExecuteScalar();

                    Console.WriteLine("DateTime Value: " + dateTimeValue);
                }

                connection.Close();
            }
        }
    }
}
