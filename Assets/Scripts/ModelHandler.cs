using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHandler : MonoBehaviour
{
    [SerializeField]
    Transform[] joints;

    [SerializeField]
    Transform self_lock;

    [SerializeField]
    Transform other_lock;

    Vector3 self_lock_pos;
    Vector3 other_lock_pos;

    float delta = 10f;

    public int n_joints = 4;

    void Start()
    {
        self_lock_pos = self_lock.transform.localPosition;
        other_lock_pos = other_lock.transform.localPosition;
    }

    public void LockChange(string lock_key, bool val)



    {
        if (val)
        {
            future_arm_state.action_map[lock_key] = LockStatus.LOCK;
        }
        else
        {
            future_arm_state.action_map[lock_key] = LockStatus.UNLOCK;
        }
        Update3dModel();
    }

    private void rotateBetween(float[] old_angles, float[] new_angles)
    {
        for (int i = 0; i < n_joints; i++)
        {
            joints[i].Rotate(0, 0, new_angles[i] - old_angles[i], Space.Self);
        }
    }

    private int delta_y = -10;
    private void UpdateLock(Transform transform, Vector3 default_pos, bool is_locked)
    {
        float new_x = is_locked ? default_pos.x : default_pos.x + delta_y;
        transform.localPosition = new Vector3(new_x, default_pos.y, default_pos.z);
    }

    public void UpdateJoint(int current_joint, int direction)
    {
        future_arm_state.angles[current_joint] += delta * direction;
    }

    private void Update3dModel()
    {
        UpdateLock(self_lock, self_lock_pos, future_arm_state.action_map["self"] == LockStatus.LOCK);
        UpdateLock(other_lock, other_lock_pos, future_arm_state.action_map["other"] == LockStatus.LOCK);
    }
    
    public ArmState GetCurrentArmState()
    {
        return current_arm_state;
    }

    public ArmState GetFutureArmState()
    {
        return future_arm_state;
    }

    void Update()
    {
        
    }
}
