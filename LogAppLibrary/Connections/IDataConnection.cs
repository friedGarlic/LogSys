namespace LogAppLibrary
{
    public interface IDataConnection
    {
        UserModel CreateUser(UserModel user_model);
        UserModel CurrentTime(UserModel userModel, PurposeModel purpose);
        UserModel CurrentTime(UserModel user_model);
        ItemModel CreateItem(ItemModel item_model);
        PurposeModel CreatePurpose(UserModel usermodel, PurposeModel purpose);
        ItemModel RemoveItem(ItemModel item_model);
        bool IsStudentIdDuplicate(UserModel usermodel);
        bool IsNotRegistered(UserModel user_model);
    }
}
