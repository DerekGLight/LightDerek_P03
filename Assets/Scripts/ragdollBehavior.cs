using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ragdollBehavior : MonoBehaviour
{
    [SerializeField] public string _team;
    [SerializeField] private string _enemyTeam;
    //[SerializeField] private float _rotationSpeed = 6;
    [SerializeField] private SkinnedMeshRenderer _skin;
    [SerializeField] private Material _redColor;
    [SerializeField] private Material _blueColor;
    [SerializeField] private ConfigurableJoint _hips;
    [SerializeField] private Animator _ragdollAnimator;

    [SerializeField] private AudioClip _hit1;
    [SerializeField] private AudioClip _hit2;
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip _victory;
    [SerializeField] AudioSource _musicPlayer;

    [SerializeField] Text _teamVictoryText;
    [SerializeField] Text _defeatedTeamTeamText;
    [SerializeField] GameObject _Canvas;

    public float _hp = 100;

    private float angle = 90;
    AudioSource _thisSource;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = _team;
        _thisSource = GetComponent<AudioSource>();

        switch (_team)
        {
            case "Red":
                _skin.material = _redColor;
                _enemyTeam = "Blue";
                break;
            case "Blue":
                _skin.material = _blueColor;
                _enemyTeam = "Red";
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject closestEnemy = FindClosestEnemy();
        
        if(closestEnemy != null && closestEnemy.tag != "Dead" && gameObject.tag != "Dead")
        {
            //Debug.Log(closestEnemy.name);
            LookAtObject(closestEnemy.transform.GetChild(0).GetChild(1)); //Look at the object
            _ragdollAnimator.SetBool("isWalking", true);
        }
        else
        {
            _ragdollAnimator.SetBool("isWalking", false);
            _ragdollAnimator.SetBool("isPunching", false);
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
        if (_hp < 0)
        {
            Die();
            _hp = 0;
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
                    if (distanceToClosest <= 2 && gameObject.tag != "Dead")
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

    private void Die()
    {
        gameObject.tag = "Dead";
        Collapse();
        _ragdollAnimator.SetBool("isWalking", false);
        _ragdollAnimator.SetBool("isPunching", false);

        _teamVictoryText.text = _enemyTeam.ToUpper() + " VICTORY!";
        _defeatedTeamTeamText.text = "Defeated " + _team + " Team";
        _Canvas.SetActive(true);

        _thisSource.Stop();
        _thisSource.PlayOneShot(_death);
        _musicPlayer.Stop();
        _musicPlayer.PlayOneShot(_victory);
    }
    private void Collapse()
    {
        ConfigurableJoint hipJoint = _hips.GetComponent<ConfigurableJoint>();

        JointDrive jDrive1 = hipJoint.angularXDrive;
        JointDrive jDrive2 = hipJoint.angularYZDrive;
        jDrive1.positionSpring = 0f;
        jDrive2.positionSpring = 0f;
        hipJoint.angularXDrive = jDrive1;
        hipJoint.angularYZDrive = jDrive2;
    }

    public void TakeDamage(float x)
    {
        _hp -= x;
    }

    public void PlayHitNoise(int i)
    {
        if(i == 1 && _thisSource.isPlaying != true)
        {
            _thisSource.PlayOneShot(_hit1);
        }
        if(i == 2 && _thisSource.isPlaying != true)
        {
            _thisSource.PlayOneShot(_hit2);
        }
    }
}
