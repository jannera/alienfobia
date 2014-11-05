using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class NPCMovement : CompleteProject.PhotonBehaviour
    {
        private NavMeshAgent nav;
        private GameObject humanPlayer;

        private float navMeshUpdateTimer = 0f;
        private static float NAVMESH_UPDATE_PERIOD = 0.3f; // in seconds
        private Vector3 nextWayPoint;
        private static float followingDistance = 6f;
        public float avoidanceRadius = 10;
        public GameObject indicatorPreFab;
        private MeshRenderer[] meshRenderers;

        private const int sectors = 8; 
        private float[] valueArr;
        private Vector2 fromMeToEnemy;
        private const int ENEMY_COST = -3;
        private Ray hitRay;
        private RaycastHit shootHit;
        private PlayerHealth ownHealth;

        PlayerMovement playerMovement;

        void Awake()
        {
            if (PlayerManager.IsNPCClient() && photonView.isMine)
            {
                nav = GetComponent<NavMeshAgent>();
                nav.Stop();
                valueArr = new float[sectors];
                fromMeToEnemy = new Vector2();
                GameObject go = (GameObject)Instantiate(indicatorPreFab, transform.position, Quaternion.identity);
                go.transform.Rotate(0, 22.5f, 0, Space.World);
                meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
                hitRay = new Ray();
                ownHealth = GetComponent<PlayerHealth>();
                playerMovement = GetComponent<PlayerMovement>();
                Debug.Log("NPC awakening!");
            }
            else
            {
                Destroy(GetComponent<NavMeshAgent>());
                Destroy(this);
                return;
            }
            nextWayPoint = transform.position;
        }

        void FixedUpdate()
        {
            if (ownHealth.isDead || ownHealth.isDowned)
            {
                return; // stop moving when dead or down
            }
            ResetValues();
            bool needMovingE = ObserveEnemies();
            bool needMovingH = ObserveHumanPlayer();
            UpdateSectorIndicators();

            if (needMovingE || needMovingH)
            {
                Move();
            }
            else
            {
                playerMovement.Move(Vector3.zero);
            }
        }

        private void Visualize(GameObject enemy)
        {
            HighlightSphereController c = enemy.GetComponentInChildren<HighlightSphereController>();
            c.Show();
        }

        private int GetRotatingIndex(int index)
        {
            while (index < 0)
            {
                index += sectors;
            }
            while (index >= sectors)
            {
                index -= sectors;
            }
            return index;
        }

        // updates the value array to avoid nearby enemies
        private bool ObserveEnemies()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, avoidanceRadius);
            bool result = false;
            foreach (Collider collider in hitColliders)
            {
                GameObject go = collider.gameObject;
                if (go.CompareTag("Enemy"))
                {
                    // raycast in order to skip enemies that are behind walls
                    Vector3 diff = go.transform.position - transform.position;
                    hitRay.origin = transform.position;
                    hitRay.direction = diff;

                    if (Physics.Raycast(hitRay, out shootHit, diff.magnitude))
                    {
                        if (!shootHit.collider.CompareTag("Player") &&
                            !shootHit.collider.CompareTag("Enemy"))
                        {
                            continue;
                        }
                    }

                    Visualize(go);

                    int sector = GetSector(go.transform.position);
                    float cost = ENEMY_COST / fromMeToEnemy.magnitude;

                    valueArr[sector] += cost;
                    valueArr[GetRotatingIndex(sector - 1)] += cost / 3f;
                    valueArr[GetRotatingIndex(sector + 1)] += cost / 3f;
                    result = true;
                }
            }
            return result;
        }

        // returns the number of sector where the given world coordinate resides in
        private int GetSector(Vector3 pos)
        {
            fromMeToEnemy.Set(pos.x - transform.position.x, pos.z - transform.position.z);
            float angle = Vector2.Angle(Vector2.up, fromMeToEnemy);
            if (fromMeToEnemy.x < 0)
            {
                angle = 360 - angle;
            }
            int sector = Mathf.FloorToInt(angle / 360f * sectors);
            return GetRotatingIndex(sector);
        }

        private bool ObserveHumanPlayer()
        {
            if (humanPlayer == null)
            {
                humanPlayer = PlayerManager.GetHumanPlayer();
            }
            if (humanPlayer == null)
            {
                return false;
            }
            navMeshUpdateTimer += Time.deltaTime;

            if ((transform.position - humanPlayer.transform.position).magnitude < followingDistance)
            {
                return false;
            }

            if (navMeshUpdateTimer > NAVMESH_UPDATE_PERIOD)
            {
                navMeshUpdateTimer = 0;
                NavMeshPath path = new NavMeshPath();
                if (nav.CalculatePath(humanPlayer.transform.position, path))
                {
                    nextWayPoint = path.corners[1];
                }
            }

            int sector = GetSector(nextWayPoint);
            // Debug.Log(diff + " -> " + sector);
            float value = ENEMY_COST / -2f;
            valueArr[sector] += value;
            valueArr[GetRotatingIndex(sector + 1)] += value / 3f * 2f;
            valueArr[GetRotatingIndex(sector - 1)] += value / 3f * 2f;
            valueArr[GetRotatingIndex(sector + 2)] += value / 3f;
            valueArr[GetRotatingIndex(sector - 2)] += value / 3f;

            string s = "";
            foreach (int i in valueArr)
            {
                s += i.ToString() + " ";
            }
            // Debug.Log(s);
            

            return true;
        }

        private void ResetValues()
        {
            for (int i = 0; i < sectors; i++)
            {
                valueArr[i] = 0;
            }
        }

        private int GetBestSector()
        {
            int result = -1;
            float highestScore = 0;
            float freeSpace = 0;
            
            for (int i = 0; i < sectors; i++)
            {
                if (result == -1 || valueArr[i] > highestScore)
                {
                    result = i;
                    freeSpace = GetFreeSpace(i);
                    highestScore = valueArr[i];
                }
                else if (valueArr[i] == highestScore)
                {
                    // if there are multiple best sectors, choose between these by doing a raycast to walls and choosing the way which has most space available
                    float newFreeSpace = GetFreeSpace(i);
                    if (newFreeSpace > freeSpace)
                    {
                        // Debug.Log("switched because of freespace " + result + " -> " + i);
                        result = i;
                        freeSpace = newFreeSpace;
                    }
                }
            }
            return result;
        }

        private float GetFreeSpace(int sector)
        {
            Vector3 p = transform.position;
            float height = transform.lossyScale.y / 2f;
            p.y += height;
            hitRay.origin = p;

            Vector3 x = GetVector(sector);
            x.y = hitRay.origin.y;
            hitRay.direction = x;
            // Debug.Log(hitRay.origin + " -> " + hitRay.direction);
            if (Physics.Raycast(hitRay, out shootHit))
            {
                // Debug.Log("hit " + shootHit.collider.gameObject);
                return (shootHit.point - hitRay.origin).magnitude;
            }
            return 0;
        }

        private Vector3 GetVector(int sector)
        {
            Vector3 result = Vector3.forward;
            float angle = (360f / sectors) * (sector + 0.5f);
            // result = Quaternion.AngleAxis(angle, Vector3.up) * result;
            result = Quaternion.Euler(0, angle, 0) * result;
            result.y = 0;
            // Debug.Log(sector + " -> " + angle + " -> " + result);
            return result;
        }

        private void Move()
        {
            int sector = GetBestSector();
            Vector3 target = GetVector(sector);

            playerMovement.Move(target);
        }

        private void UpdateSectorIndicators()
        {
            float highest = Mathf.Max(valueArr);
            float lowest = Mathf.Min(valueArr);

            for (int i = 0; i < sectors; i++)
            {
                Color c;
                if (valueArr[i] < 0)
                {
                    // if we've got a negative value, lowest can't be zero
                    c = new Color(valueArr[i] / lowest, 0, 0);
                }
                else if (valueArr[i] > 0)
                {
                    // if we've got a positive value, highest can't be zero
                    c = new Color(0, valueArr[i] / highest, 0);
                }
                else
                {
                    c = Color.gray;
                }
                meshRenderers[i].material.color = c;
            }
        }
    }

}