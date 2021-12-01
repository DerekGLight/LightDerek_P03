using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollBehavior : MonoBehaviour
{
    [SerializeField] public string _team;
    //[SerializeField] private float _rotationSpeed = 6;
    [SerializeField] private SkinnedMeshRenderer _skin;
    [SerializeField] private Material _redColor;
    [SerializeField] private Material _blueColor;
    [SerializeField] private ConfigurableJoint _hips;
    [SerializeField] private Animator _ragdollAnimator;

    public float _hp = 100;

    private float angle = 90;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = _team;

        switch (_team)
        {
            case "Red":
                _skin.material = _redColor;
                break;
            case "Blue":
                _skin.material = _blueColor;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject closestEnemy = FindClosestEnemy();
        
        if(closestEnemy != null)
        {
            //Debug.Log(closestEnemy.name);
            LookAtObject(closestEnemy.transform.GetChild(0).GetChild(1));
            _ragdollAnimator.SetBool("isWalking", true);
        }
        else
        {
            _ragdollAnimator.SetBool("isWalking", false);
        }
        if (Input.GetKey(KeyCode.F))
        {
            _hips.targetRotation = Quaternion.Euler(0, angle, 0); //Testing rotation of fighters
            angle++;
        }
        if (_hips.transform.position.y < -200)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        
        if(_team == "Red")
        {
            enemies = GameObject.FindGameObjectsWithTag("Blue");
            
            GameObject closest = null;
            float distanceToClosest = Mathf.Infinity;
            Vector3 currentPosition = transform.GetChild(0).GetChild(1).position;

            foreach(GameObject enemy in enemies)
            {
                float currentDistance = Vector3.Distance(currentPosition, enemy.transform.GetChild(0).GetChild(1).position);
                if(currentDistance < distanceToClosest)
                {
                    closest = enemy;
                    distanceToClosest = currentDistance;
                    if (distanceToClosest <= 2)
                    {
                        _ragdollAnimator.SetBool("isPunching", true);
                    }
                    else
                    {
                        _ragdollAnimator.SetBool("isPunching", false);
                    }
                }
            }
            return closest;
        }

        if (_team == "Blue")
        {
            enemies = GameObject.FindGameObjectsWithTag("Red");

            GameObject closest = null;
            float distanceToClosest = Mathf.Infinity;
            Vector3 currentPosition = transform.GetChild(0).GetChild(1).position;

            foreach (GameObject enemy in enemies)
            {
                float currentDistance = Vector3.Distance(currentPosition, enemy.transform.GetChild(0).GetChild(1).position);
                if (currentDistance < distanceToClosest)
                {
                    closest = enemy;
                    distanceToClosest = currentDistance;
                    if(distanceToClosest <= 2)
                    {
                        _ragdollAnimator.SetBool("isPunching", true);
                    }
                    else
                    {
                        _ragdollAnimator.SetBool("isPunching", false);
                    }
                }
            }
            return closest;
        }

        return null;
    }

    private void LookAtObject(Transform obj)
    {
        Vector3 targetDir = obj.position - transform.GetChild(0).GetChild(1).position;
        //Debug.Log("Hip position: " + transform.GetChild(0).GetChild(1).position);
        //Debug.Log("Enemy read Hip position: " + obj.position);
        float arcTangent = Mathf.Atan2(targetDir.z, targetDir.x) * 100;
        if(arcTangent < 0)
        {
            arcTangent = arcTangent * -1;
        }
        arcTangent += 180;
        //float step = _rotationSpeed * Time.deltaTime;
        //Vector3 newDir = Vector3.RotateTowards(_hips.targetRotation.eulerAngles, targetDir, step, 0.0f);
        //Debug.Log(_team + " target direction: " + targetDir);
        //Debug.Log(_team + " target direction angle: " + arcTangent);
        //Debug.Log(_team + ": " + newDir);
        _hips.targetRotation = Quaternion.Euler(0, arcTangent, 0);
    }

    public void TakeDamage(float x)
    {
        _hp -= x;
        //Debug.Log("TakeDamage function called");
    }
}
