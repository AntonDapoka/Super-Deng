
public interface IFaceScript 
{
    bool IsTurnOn { get; set; }
    int PathObjectCount { get; set; }
    FaceStateScript FaceState { get; }
    FaceArrayScript FaceArray { get; }
    FacePresenterScript FacePresenter { get; }

    void Initialize();
}
