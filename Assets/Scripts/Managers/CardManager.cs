using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CardManager : Singleton<CardManager>
    {
        [SerializeField] private GameObject BlackPrefab;
        [SerializeField] private GameObject BluePrefab;
        [SerializeField] private GameObject BrownPrefab;
        [SerializeField] private GameObject GreenPrefab;
        [SerializeField] private GameObject OrangePrefab;
        [SerializeField] private GameObject PurplePrefab;
        [SerializeField] private GameObject WhitePrefab;
        [SerializeField] private GameObject YellowPrefab;
        [SerializeField] private GameObject RainbowPrefab;

        private void Awake()
        {
        }
        /// <summary>
        /// Gets an instance of the given prefab from the pool. The prefab must be registered to the pool.
        /// </summary>
        /// <remarks>
        /// To spawn a NetworkObject from one of the pools, this must be called on the server, then the instance
        /// returned from it must be spawned on the server. This method will then also be called on the client by the
        /// PooledPrefabInstanceHandler when the client receives a spawn message for a prefab that has been registered
        /// here.
        /// </remarks>
        /// <param name="prefab"></param>
        /// <param name="position">The position to spawn the object at.</param>
        /// <param name="rotation">The rotation to spawn the object with.</param>
        /// <returns></returns>
        //private void Update()
        //{
        //    if (!IsServer)
        //    {
        //        return;
        //    }
        //    if (Input.GetKeyUp(KeyCode.O))
        //    {
        //        Vector3 pos = new Vector3(0, 5, 0);
        //        NetworkObject spawnedObjectTransform = NetworkObjectPool.Instance.GetNetworkObject(BlackPrefab,pos, Quaternion.identity);
        //        //spawnedObjectTransform.position = new Vector3(0, 5, 0);
        //        spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        //    }
        //}

    }
}
