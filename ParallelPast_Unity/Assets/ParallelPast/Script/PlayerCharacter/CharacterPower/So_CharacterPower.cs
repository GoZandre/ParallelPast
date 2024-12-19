using UnityEngine;

public class So_CharacterPower : ScriptableObject
{   

    protected PlayerController _playerController;

    protected ScarfRenderer _scarfController;

    [SerializeField]
    private RuntimeAnimatorController _powerAnimator;

    [SerializeField]
    private Texture _scarfSprite;

    [SerializeField]
    private Color _scarfParticleColorA;

    public Texture ScarfSprite => _scarfSprite;
    public Color ScarfColorA => _scarfParticleColorA;

    public float DebugHeight;

    [SerializeField]
    private Sprite _powerIcon;

    public Sprite PowerIcon => _powerIcon;

    //Public methods
    public virtual void EnablePower(PlayerController playerController, bool doTransform = true) 
    {

        //ScriptableObject.CreateInstance<So_CharacterPower>();

        _playerController = playerController;
        _playerController.Animator.runtimeAnimatorController = _powerAnimator;

        if(doTransform)
        {
            _playerController.Animator.SetTrigger("DoTransform");
        }

        _scarfController = _playerController.GetComponent<GhostManager>().ScarfRenderer;

        _scarfController.SwitchScarfAppearance(this);
    }

    public virtual void InvokePower() 
    {
        _playerController.Animator.SetTrigger("ActivePower");
    }

    public virtual void CancelPower() 
    {
        _playerController.Animator.SetTrigger("DeactivePower");
    }
}
