namespace SgtMapper {
  public interface ExceptionMapper<T> {
    object Map(T exception);
  }
}
