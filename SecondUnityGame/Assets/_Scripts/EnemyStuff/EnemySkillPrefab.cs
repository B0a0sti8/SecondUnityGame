using UnityEngine;

public class EnemySkillPrefab : MonoBehaviour
{
    [SerializeField] protected Sprite skillSprite;
    protected EnemyToken myEnemyToken;

    protected virtual void Start()
    {
        myEnemyToken = transform.parent.parent.GetComponent<EnemyToken>();
    }

    public virtual void UseSkill()
    {

    }
}
