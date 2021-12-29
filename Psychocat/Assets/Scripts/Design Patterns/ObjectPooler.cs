using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool 
    {
        public string name;
        public GameObject whatToSpawn;
        public int poolSize;
        public GameObject parent;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary; //um dicionario, armazena varias queue, ao inves de buscar-lás por posição vc busca por string;

    #region Singleton

    //singletonzinho pra acessar facinho
    public static ObjectPooler instance; 
   
    private void Awake()
    {
        instance = this;
    }
    #endregion Singleton
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools) // pra cada item na lista de pools....
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); //vai ser criada a queue que vai jogar no dicionario

            for (int  i = 0;  i < pool.poolSize;  i++)
            {
                GameObject obj = Instantiate(pool.whatToSpawn); // guarda para poder desligar
                if (pool.parent != null)
                {
                    obj.transform.parent = pool.parent.transform;
                }
                obj.SetActive(false); //desligando
                objectPool.Enqueue(obj);//guarda na queue
            }

            poolDictionary.Add(pool.name, objectPool); //adiocionando a queue criada com o nome especificado
        }
    }

    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        //game object no retorno para caso alguem queira mexer no objeto que foi spawnado
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.Log("There is no pool called" + name);
            return null;
        }

        GameObject obj = poolDictionary[name].Dequeue(); // tirando o primeiro da queue e guardando pra poder mexer
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        //como o start só roda quando o objeto é criado a primeira vez, foi criado essa interface com o metodo que vai ser
        //chamado quando ele é ativado

        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        else
        {
            Debug.Log("The object in the " + name + " pool, doesn't have a OnObjectSpawn");
        }

        poolDictionary[name].Enqueue(obj); // colocando o objeto recem spawnado de volta na queue, ou seja, agr ele é o ultimo
                                           //dessa forma, ele so vai ser spawnado denovo quando acabar todos os outros da pool
        return obj;
    }
       
}
