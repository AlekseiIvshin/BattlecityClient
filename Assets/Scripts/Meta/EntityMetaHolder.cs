using UnityEngine;

public class EntityMetaHolder : MonoBehaviour
{
    private int entityId = -1;

    public void setEntityId(int id)
    {
        this.entityId = id;
    }

    public int getEntityId()
    {
        return entityId;
    }
}
