using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �־û��洢�����غ����������
/// </summary>
public class DataHolder : MonoBehaviour
{
    // ����ʵ��
    private static DataHolder instance;

    // �������������б�
    public List<WeaponData> availableitemList = new List<WeaponData>();
    // ��������������б�
    public List<WeaponData> playerWeaponList = new List<WeaponData>();

    // ���ü��ܿ��б�
    public List<SkillData> availableSkillList = new List<SkillData>();
    // ��Ҽ��ܿ��б�
    public LinkedList<SkillData> skillList = new LinkedList<SkillData>();

    // ����ˢ�µ��Ѷ�����
    public DifficultyCurve difficultyCurve;

    // ����һ�����������ڼ�¼������Ϸ�еĵ�������
    public int enemyCount = 0;

    // ����һ��ȫ��ѹ��ֵ
    public float stressValue = 0;
    //����һ��ȫ�ֵ������ӵ��ȼ�����Ϊ��ʱֻ��һ��������
    public int bulletLevel = 1;
    public int gametimes = 1;

    // ��ȡ����ʵ��
    public static DataHolder Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataHolder>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("WeaponDataHolder");
                    instance = obj.AddComponent<DataHolder>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // ȷ��ֻ��һ��ʵ������
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �ڴ˴���������������Ҫ�־û��洢�����ݺͷ���
    // ��ȡ������ָ��λ�õ���
    public SkillData GetSkillAtPosition(int position)
    {
        int index = 0;
        foreach (SkillData item in skillList)
        {
            if (index == position)
            {
                return item;
            }
            index++;
        }
        return null; // ���ָ��λ�ò�����
    }
}
 