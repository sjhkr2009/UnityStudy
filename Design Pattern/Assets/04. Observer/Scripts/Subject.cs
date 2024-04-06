namespace Observer {
    public interface ISubject<T> {
        void AddObserver(IObserver<T> observer);
        void RemoveObserver(IObserver<T> observer);
        void Notify();
    }
}
