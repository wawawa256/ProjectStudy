using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectSystem : MonoBehaviour
{
    [SerializeField] PlayerController player;
    List<FlowChartObject> objects = new List<FlowChartObject>();
    List<IfObject> ifObjects = new List<IfObject>();

    GameObject[] objectPrefabs;
    public void AddNewObject(FlowChartObject obj)//どこに？は必要そう
    {
        objects.Add(obj);
        if (obj is IfObject)
        {
            ifObjects.Add((IfObject)obj);
        }
    }

    void RemoveObject(int code)
    {
        objects.RemoveAt(code);
    }

    void Display()
    {
        //Listから全てをInstantiateして表示する
        //実験的
        int count = 0;
        for (int i = 0; i < objects.Count; i++)
        {
            FlowChartObject obj = objects[i];
            if (obj.Prefab != null)
            {
                objectPrefabs[count] = Instantiate(obj.Prefab, new Vector3(0, -1.5f * count + 1.0f, 0), Quaternion.identity);
                count++;
            }
        }

        //GetSize();
    }

    void GetSize()
    {
        //ifのsizeに応じてFlowのサイズを変更する
        int size = 0;
        foreach (IfObject obj in ifObjects)
        {
            if (obj is IfTrueObject)
            {
                size += ((IfTrueObject)obj).Size;
            }
            else if(obj is IfFalseObject)
            {
                size++;
            }
            else if(obj is IfEndObject)
            {
                size++;
            }
        }
        Debug.Log(size);
    }
    void ResizeFlow()
    {
        //ifのsizeに応じてFlowのサイズを変更する
    }

    void Reload()
    {
        foreach (GameObject obj in objectPrefabs)
        {
            Destroy(obj);
        }
    }

    void Start()
    {
        Init();
        AddNewObject(new SkillObject(player.Battler.GetSkill(), 1));
        AddNewObject(new SkillObject(player.Battler.GetSkill(), 2));
        AddNewObject(new IfTrueObject(3));
        AddNewObject(new IfTrueObject(4));
        AddNewObject(new SkillObject(player.Battler.GetSkill(), 8));
        AddNewObject(new IfFalseObject(4));
        AddNewObject(new SkillObject(player.Battler.GetSkill(), 7));
        AddNewObject(new IfEndObject(4));
        AddNewObject(new IfFalseObject(3));
        AddNewObject(new SkillObject(player.Battler.GetSkill(), 5));
        AddNewObject(new IfEndObject(3));
        AddNewObject(new SkillObject(player.Battler.GetSkill(), 6));
        //idは統一したものを使います
        Display();
    }
    void Init()
    {
        objectPrefabs = new GameObject[Constant.MAX_OBJECTS];
    }
}
