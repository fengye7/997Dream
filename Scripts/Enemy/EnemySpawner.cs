using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifficultyCurve
{
    public float spawnIntervalDecreaseRate = 0.1f; // ��Сˢ�¼��������
    public int maxEnemyCountIncreaseRate = 1; // ÿ�����ӵ�����������
    public float difficultyIncreaseRate = 0.1f; // ÿ���Ѷ����ӵ���

    public float spwanInterval = 15f;  //ÿ����ˢ�¼��
    public int maxEnemyCount = 1;
    public float difficultyValue = 1;
    public int currentDifficultyLevel = 1; // ��ǰ�Ѷȼ���

    // �������캯��
    public DifficultyCurve(float spawnIntervalDecreaseRate, int maxEnemyCountIncreaseRate, float difficultyIncreaseRate)
    {
        this.spawnIntervalDecreaseRate = spawnIntervalDecreaseRate;
        this.maxEnemyCountIncreaseRate = maxEnemyCountIncreaseRate;
        this.difficultyIncreaseRate = difficultyIncreaseRate;
        this.spwanInterval = 3f; // Ĭ��ˢ�¼��
        this.maxEnemyCount = 1; // Ĭ������������
        this.difficultyValue = 1;
        this.currentDifficultyLevel = 1;
    }
}

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // �洢��ͬ�Ѷ��µĵ���Ԥ����
    public Transform playerTransform;
    public float randomRadius = 12f; // ���ˢ�ֵİ뾶


    [Header("ScaleInfo")]
    public float minScale = 0.9f;
    public float scaleDuration = 0.01f;
    private void Start()
    {
        DataHolder.Instance.difficultyCurve = new DifficultyCurve(0.1f,1,0.1f);

        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            // ����ҵ���Ұ��Χ��������ɹ���
            // ��������Ϸ�еĵ�������С��������������ʱ�����µĵ���
            if (DataHolder.Instance.enemyCount < DataHolder.Instance.difficultyCurve.maxEnemyCount)
            {
                int spwanNum = Random.Range(0, DataHolder.Instance.difficultyCurve.maxEnemyCount - DataHolder.Instance.enemyCount);
                for (int i = 0; i < spwanNum; i++)
                {
                    // ���ݵ�ǰ�Ѷȼ������ѡ�����Ԥ����
                    int enemyIndex = Random.Range(0, Mathf.Min(DataHolder.Instance.difficultyCurve.currentDifficultyLevel, enemyPrefabs.Count));
                    GameObject enemyPrefab = enemyPrefabs[enemyIndex];

                    Vector3 randomPosition = new(Random.Range(-randomRadius, randomRadius), Random.Range(-randomRadius, randomRadius), 0f);
                    GameObject midEnemy =Instantiate(enemyPrefab, playerTransform.position + randomPosition, Quaternion.identity);
                    Utils.ScaleEffect(midEnemy.transform, minScale, scaleDuration);
                }
            }
            Debug.Log("��ǰ����������" + DataHolder.Instance.enemyCount);
            Debug.Log("��ǰ����������ƣ�" + DataHolder.Instance.difficultyCurve.maxEnemyCount);

            yield return new WaitForSeconds(DataHolder.Instance.difficultyCurve.spwanInterval);

            // �����Ѷ����ߵ���ˢ�¼��������������
            AdjustDifficulty();
        }
    }

    private void AdjustDifficulty()
    {
        //spawnInterval -= spawnIntervalDecreaseRate; // ��Сˢ�¼��===������־���ʽˢ��
        DataHolder.Instance.difficultyCurve.maxEnemyCount += DataHolder.Instance.difficultyCurve.maxEnemyCountIncreaseRate; // ��������������
        DataHolder.Instance.difficultyCurve.difficultyValue += DataHolder.Instance.difficultyCurve.difficultyIncreaseRate;  // �Ѷ�����������

        if (Mathf.FloorToInt(DataHolder.Instance.difficultyCurve.difficultyValue + 0.5f) > Mathf.FloorToInt(DataHolder.Instance.difficultyCurve.difficultyValue))
        {
            // ����ﵽ��һ���������������Ѷȼ���
            DataHolder.Instance.difficultyCurve.currentDifficultyLevel++;
        }
    }
}
