/// <summary>
/// 角色控制的有限状态机的接口
/// </summary>
public interface PlayerIState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}
