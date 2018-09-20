using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerObject : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        var entityMeta = gameObject.GetComponent<EntityMetaHolder>();
        ObjectDestroyEventManager.getInstance().destroyEntity(entityMeta.getEntityId());
    }
}
