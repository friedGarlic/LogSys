namespace LogAppLibrary
{
    public interface IDataConnection
    {
        UserModel CreateUser(UserModel user_model);
        UserModel CurrentTime(UserModel userModel, PurposeModel purpose);
        UserModel CurrentTime(UserModel user_model);

        ItemModel CreateItem(ItemModel item_model);
        ItemModel AddQuantityItem(ItemModel item_model);

        ItemModel RemoveItem(ItemModel item_model);
        ItemModel RemoveItemName(ItemModel item_model);

        PurposeModel CreatePurpose(UserModel usermodel, PurposeModel purpose);

        bool IsItemDuplicate(ItemModel item_model);
        bool IsStudentIdDuplicate(UserModel usermodel);

        void AddUnreturnedItem(string name, decimal q);
        void SubUnreturnedItem(string name, decimal q);
    }
}
