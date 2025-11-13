
public interface IFaceScript 
{
    FaceStateScript FaceState { get; }
    FaceArrayScript FaceArray { get; }
    FacePresenterScript FacePresenter { get; }

    void Initialize();
}
