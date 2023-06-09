using Dapper;
using System.Data;

namespace LogAppLibrary
{
    public class SqlConnector : IDataConnection
    {
        public UserModel CreateUser(UserModel user_model)
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

        public UserModel CurrentTime(UserModel user_model)
        {
            using (IDbConnection dbConnection = new System.Data.SqlClient.SqlConnection(GlobalConfig.ConnectString("StudentsDB")))
            {
                var p = new DynamicParameters();
                p.Add("@StudentIdNumber", user_model.student_ID);

                return user_model;
            }
        }

        public UserModel CurrentTime(UserModel user_model, PurposeModel purposeModel)
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
        public ItemModel CreateItem(ItemModel item_model)
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
    }
}
