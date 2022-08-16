using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField]
    private RingDatabase ringDatabase;

    [SerializeField]
    private CirclePrefab circlePrefab;

    [SerializeField]
    private Transform table;

    [SerializeField]
    private Button rollBtn;

    [SerializeField]
    private TMPro.TextMeshProUGUI pointText;

    [SerializeField]
    private EffectLineController effectRow;
    [SerializeField]
    private EffectLineController effectCollum;
    [SerializeField]
    private EffectLineController effectDiaginalLeft;
    [SerializeField]
    private EffectLineController effectDiaginalRight;
    [SerializeField]
    private ColorDatabase colorDatabase;

    private List<CirclePrefab> circlePrefabs;
    private Dictionary<int, List<CirclePrefab>> listRow = new Dictionary<int, List<CirclePrefab>>();
    private Dictionary<int, List<CirclePrefab>> listColumn = new Dictionary<int, List<CirclePrefab>>();
    private Dictionary<int, CirclePrefab> listDiagonalLeft= new Dictionary<int, CirclePrefab>();
    private Dictionary<int, CirclePrefab> listDiagonalRight = new Dictionary<int, CirclePrefab>();

    private int width = 3;
    private int point;
    private int numberCircle = 1;
    private int numberColor = 3;
    private int numberRoll = 0;
    private bool isLose;

    private List<ColorType> colorTypes;
    private List<CircleSizeType> sizeTypes;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        circlePrefabs = new List<CirclePrefab>();

        AddListener();
        Init();
        RandomCircle();
        point = 0;
        pointText.text = point.ToString();
        numberCircle = 1;
        numberRoll = 0;
        isLose = false;

    }

    private void AddListener()
    {
        rollBtn.onClick.AddListener(OnClick_Roll);
    }

    private void Init()
    {
        for(int i = 0; i < width * width; i++)
        {
            CirclePrefab circle = Instantiate(circlePrefab, table);
            circle.name = i.ToString();
            circle.setIndex(i);
            circlePrefabs.Add(circle);
        }

        for (int i = 0; i < circlePrefabs.Count; i++)
        {
            int row = i / 3;
            if(listRow.ContainsKey(row))
            {
                listRow[row].Add(circlePrefabs[i]);
            }
            else
            {
                List<CirclePrefab> newList = new List<CirclePrefab>();
                newList.Add(circlePrefabs[i]);
                listRow.Add(row, newList);
            }

            int column = i % 3;
            if (listColumn.ContainsKey(column))
            {
                listColumn[column].Add(circlePrefabs[i]);
            }
            else
            {
                List<CirclePrefab> newList = new List<CirclePrefab>();
                newList.Add(circlePrefabs[i]);
                listColumn.Add(column, newList);
            }

            if(row == column)
            {
                listDiagonalLeft.Add(i, circlePrefabs[i]);
            }

            if(row + column == 2)
            {
                listDiagonalRight.Add(i, circlePrefabs[i]);
            }
        }

        colorTypes = ringDatabase.GetListColor();
        sizeTypes = ringDatabase.GetListSize();
    }

    public void RandomCircle()
    {
        List<CirclePrefab> listRandom = new List<CirclePrefab>();

        foreach(CirclePrefab circlePrefab in circlePrefabs)
        {
            if(!circlePrefab.IsFull)
            {
                listRandom.Add(circlePrefab);
            }
        }

        if(listRandom.Count == 0)
        {
            SetLose();
            return;
        }

        int randomCell = Random.Range(0, listRandom.Count);
        CirclePrefab circle = listRandom[randomCell];

        Debug.Log("Circle random: " + circle.GetIndex());

        List<RingParameter> circles = new List<RingParameter>();
        List<CircleSizeType> newSize = new List<CircleSizeType>();
        newSize.Add(CircleSizeType.Big);
        newSize.Add(CircleSizeType.Normal);
        newSize.Add(CircleSizeType.Small);
        
        foreach(CircleSizeType circleSizeType in circle.GetCircleActive())
        {
            newSize.Remove(circleSizeType);
        }

        int randomCircle;
        if(numberCircle > newSize.Count)
        {
            randomCircle = Random.Range(1, newSize.Count + 1);
        }
        else
        {
            randomCircle = Random.Range(1, numberCircle + 1);
        }

        for (int i = 0; i < randomCircle; i++)
        {
            int sizeType = Random.Range(0, newSize.Count);
            Debug.Log("Tan new size " + sizeType + " || size count: " + newSize.Count);
            ColorType colorType = randomColor();
            circles.Add(ringDatabase.GetRingByColorAndSize(colorType, newSize[sizeType]));
            newSize.RemoveAt(sizeType);
        }
        CircleRandom.instance.ClearAllCircle();
        CircleRandom.instance.SetCircle(circles);
    }

    public ColorType randomColor()
    {
        int colorRandom = Random.Range(0, numberColor);
        return colorTypes[colorRandom];
    }

    public void CheckPoint(CirclePrefab cirlcle, List<ColorType> colors)
    {
        int index = cirlcle.GetIndex();
        int row = index / 3;
        int column = index % 3;

        foreach(ColorType color in colors)
        {
            bool checkRow = CheckColor(listRow[row], color);
            bool checkColumn = CheckColor(listColumn[column], color);
            bool checkDiagonalLeft = false;
            bool checkDiagonalRight = false;
            int count = 0;

            Color color1 = GetColorByColorType(color);

            if(listDiagonalLeft.ContainsKey(index))
            {
                checkDiagonalLeft = CheckColor(listDiagonalLeft.Values.ToList(), color);
            }

            if (listDiagonalRight.ContainsKey(index))
            {
                checkDiagonalRight =  CheckColor(listDiagonalRight.Values.ToList(), color);
            }

            if (checkRow)
            {
                foreach(CirclePrefab circlePrefab in listRow[row])
                {
                    count += circlePrefab.ClearColor(color);
                }
                effectRow.transform.localPosition = new Vector3(effectRow.transform.localPosition.x, cirlcle.transform.localPosition.y, effectRow.transform.localPosition.z);
                effectRow.PlayEffect(color1);
            }

            if (checkColumn)
            {
                foreach (CirclePrefab circlePrefab in listColumn[column])
                {
                    count += circlePrefab.ClearColor(color);
                }
                effectCollum.transform.localPosition = new Vector3(cirlcle.transform.localPosition.x, effectCollum.transform.localPosition.y, effectCollum.transform.localPosition.z);
                effectCollum.PlayEffect(color1);

            }

            if(checkDiagonalLeft)
            {
                foreach (CirclePrefab circlePrefab in listDiagonalLeft.Values)
                {
                    count += circlePrefab.ClearColor(color);
                }
                effectDiaginalLeft.PlayEffect(color1);
            }

            if (checkDiagonalRight)
            {
                foreach (CirclePrefab circlePrefab in listDiagonalRight.Values)
                {
                    count += circlePrefab.ClearColor(color);
                }
                effectDiaginalRight.PlayEffect(color1);
            }

            if(cirlcle.CheckColor())
            {
                count += 3;
                cirlcle.ClearAllCircle();
            }

            point += count * 100;
            pointText.text = point.ToString();

            if(point > 500)
            {
                numberCircle = 2;
                numberColor = 4;
            }    

            if(point > 1000)
            {
                numberColor = 5;
            }
        }

        numberRoll = 0;
    }

    public void OnClick_Roll()
    {
        if (!isLose)
        {
            numberRoll++;

            if (numberRoll > 5)
            {
                SetLose();
                return;
            }

            RandomCircle();
        }
        else
        {
            numberRoll = 0;
            isLose = false;
            foreach (CirclePrefab circlePrefab in circlePrefabs)
            {
                circlePrefab.ClearAllCircle();
            }
        }
    }

    public Color GetColorByColorType(ColorType colorType)
    {
        return colorDatabase.GetColorByColorType(colorType).color;
    }    

    private bool CheckColor(List<CirclePrefab> listToCheck, ColorType color)
    {
        foreach (CirclePrefab circlePrefab in listToCheck)
        {
            if (!circlePrefab.ContainsColor(color))
            {
                return false;
            }
        }
        return true;
    }

    private void SetLose()
    {
        isLose = true;
        pointText.text = "YOU LOSE!";
    }

    private void Restart()
    {

    }
}
