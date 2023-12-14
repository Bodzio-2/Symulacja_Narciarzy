using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class HeatMapDumper : MonoBehaviour
{
    List<Vector3> playerPositions = new List<Vector3>();

    public float heatmapGenerationCooldown = 10f;

    private float heatmapTimer = 0.0f;
    public string savePath = "C:\\Users\\dunge\\Desktop\\Game Projects\\SSD Prototyping\\Heatmap_data\\";
    int i = 0;

    [XmlRoot("Vector3Collection")]
    public class Vector3Container
    {
        [XmlArray("Vectors"), XmlArrayItem("Vector")]
        public List<Vector3> vectors = new List<Vector3>();

        public Vector3Container(List<Vector3> _vectors)
        {
            vectors = _vectors;
        }

        public Vector3Container() { }
    }

   

    private void Update()
    {
        if(Time.time > heatmapTimer)
        {
            heatmapTimer = Time.time + heatmapGenerationCooldown;
            AppendData();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            i++;
            DumpDataCsv();
        }
    }

    private void AppendData()
    {
        GameObject[] skiers = GameObject.FindGameObjectsWithTag("Skier");

        foreach(GameObject skier in skiers)
        {
            playerPositions.Add(new Vector3(skier.transform.position.x, 0, skier.transform.position.z));
        }
    }

    public void DumpDataXml(int i)
    {
        Vector3Container container = new Vector3Container(playerPositions);

        
        XmlSerializer serializer = new XmlSerializer(typeof(Vector3Container));
        System.DateTime curDate = System.DateTime.Now;
        FileStream stream = new FileStream(savePath+"Heatmap-Dump"+curDate.Year+"_"+curDate.Month+"_"+curDate.Day+"_"+curDate.Hour+"_"+curDate.Second+".xml", FileMode.Create);
        serializer.Serialize(stream, container);
        stream.Close();

        Debug.Log("Data has been dumped!");
    }

    public void DumpDataCsv()
    {
        System.DateTime curDate = System.DateTime.Now;
        string path = savePath + "Heatmap-Dump" + curDate.Year + "_" + curDate.Month + "_" + curDate.Day + "_" + curDate.Hour + "_" + curDate.Minute + "_" + curDate.Second + ".csv";

        StringBuilder csv = new StringBuilder();
        foreach(Vector3 pos in playerPositions)
        {
            float _x = pos.x;
            float _y = pos.y;
            float _z = pos.z;
            string newLine = string.Format("{0}; {1}; {2}", _x, _y, _z);
            csv.AppendLine(newLine);
        }
        File.WriteAllText(path, csv.ToString());

        Debug.Log("Dumped to CSV");
    }

    
}
