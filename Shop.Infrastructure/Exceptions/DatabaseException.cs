namespace Shop.Infrastructure
{
    public class DatabaseException:Exception
    {
        public DatabaseException():base("Can't save to database. Entity framework internal exception") 
        { }
    }
}
