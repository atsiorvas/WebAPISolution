namespace Common.Interface {
    public interface IRepository<T> where T : Entity {

        ISaveChangesWarper SaveChangesWarper { get; }
    }
}