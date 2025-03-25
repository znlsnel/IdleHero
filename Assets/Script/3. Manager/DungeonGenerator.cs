using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[Serializable]
public class BuildElements
{
    public GameObject prefab;
    public int sizeX;
    public int sizeZ;
    public int stairHeight; 
}
 

public class DungeonGenerator : MonoBehaviour
{
    // Build Elements
    [SerializeField] private BuildElements _floor;
    [SerializeField] private BuildElements railing;
    [SerializeField] private BuildElements _column; 
    [SerializeField] private BuildElements _stair;
    [SerializeField] private BuildElements _corridor;

    // Dungeon Size
    [SerializeField] private Vector2 _dungeonSize;
    [SerializeField] private int _dungeonDepth;

    // Dungeon Node
    private List<Vector3> _nodes = new List<Vector3>();
    private List<(int, int)> _edges = new List<(int, int)>();
    void Awake()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        // 던전 생성 로직
        // 노드 생성
        CreateNode();

        // 노드 배치
        PlaceNodes();

        // 노드 연결
        ConnectNodes();

        // 노드 경로 생성
        GeneratePath(); 

        // 난간 설치
        GenerateRailing();
    }

    private void CreateNode()
    {
        _nodes.Clear(); 
        bool [,] _visited = new bool[(int)_dungeonSize.x, (int)_dungeonSize.y];

        int nodeCnt = _dungeonDepth;
        while (nodeCnt > 0)
        {
            int x = Random.Range(0, (int)_dungeonSize.x);
            int z = Random.Range(0, (int)_dungeonSize.y);

            if (!_visited[x, z])
            {
                _nodes.Add(new Vector3(x, 0, z)); 
                _visited[x, z] = true;
                nodeCnt--;
            } 
        }
    }

    private void PlaceNodes()
    {
        foreach (var node in _nodes)
        {
            GameObject floor1 = Instantiate(_floor.prefab, node*8 - new Vector3(_floor.sizeX, 0, _floor.sizeZ), Quaternion.identity);
        }
    }

    private void ConnectNodes()
    {
        // from, to, cost ==> ex((1, 2, 3) => 1에서 2로 가는 거리는 3)
        List<(int, int, int)> tempEdges = new List<(int, int, int)>();
        // 최종 간선을 담을 List
        _edges.Clear();

        // 비용 초기화
        for (int i = 0; i < _nodes.Count; i++)
            for (int j = i+1; j < _nodes.Count; j++)
                tempEdges.Add((i, j, (int)Vector3.Distance(_nodes[i], _nodes[j])));
            
        // 비용 오름차순 정렬
        tempEdges.Sort((a, b) => a.Item3.CompareTo(b.Item3));

        // 크루스칼 알고리즘을 통해 최소 간선만 남기기
        // Union - Find 알고리즘을 통해 간선 연결 여부 파악
        int[] parents = new int[_nodes.Count];
        for (int i = 0; i < _nodes.Count; i++)
            parents[i] = i;


        // 최소 간선만 남기기
        for (int i = 0; i < tempEdges.Count; i++)
        {
            var (From, To, Cost) = tempEdges[i];
            
            // 이미 연결 되었습니다.
            if (Find(parents, From) == Find(parents, To))
                continue; 

            Union(parents, From, To);
            _edges.Add((From, To));
        }
    } 

    private void GeneratePath()
    {
        HashSet<Vector3> visited = new HashSet<Vector3>();
        foreach (var edge in _edges)
        {
            var (From, To) = edge;
            Vector3 start = _nodes[From];
            Vector3 end = _nodes[To];

            // 간단한 방법으로 길찾기 수행
            // 장애물이 없는 상황이기 때문에 상관 X

            // A -> B까지 가기 위한 현재의 선택지는 2개
            // 직진 or (우, 좌)
            // X값이 같은 상황? ==> Z값에 대한 이동
            // Z값이 같은 상황? ==> X값에 대한 이동

            // 둘다 다른 상황 ==> 둘중 한 방향을 선택

            while (true)
            {
                int dirZ = end.z == start.z ? 0 : end.z - start.z > 0 ? 1 : -1;
                int dirX = end.x == start.x ? 0 : end.x - start.x > 0 ? 1 : -1;
 
                if (dirZ == 0)
                    start.x += dirX;
                
                else if (dirX == 0)
                    start.z += dirZ;

                else
                {
                    bool isMoveToXDir = Random.Range(0, 2) == 0;
                    if (isMoveToXDir)
                        start.x += dirX;
                    else
                        start.z += dirZ;
                }

                if (start == end)
                    break;

                if (visited.Contains(start))
                    continue;

                visited.Add(start);

                GameObject floor1 = Instantiate(_floor.prefab, start*8 - new Vector3(_floor.sizeX, 0, _floor.sizeZ), Quaternion.identity);
                _nodes.Add(start); 
            }
        } 
    } 

    private void GenerateRailing()
    {
        HashSet<Vector2> nodeHash = new HashSet<Vector2>();

        foreach (var node in _nodes)
            nodeHash.Add(new Vector2(node.x, node.z));
 
        for (int i = 0; i < _nodes.Count; i++)
        {
            Vector3 curNode = _nodes[i] * 8; 
            curNode -= new Vector3(_floor.sizeX, 0, _floor.sizeZ);


            // 왼쪽에 다른 노드가 있는가?
            if (!nodeHash.Contains(new Vector2(_nodes[i].x - 1, _nodes[i].z)))
            {
                var go = Instantiate(railing.prefab, curNode + new Vector3(-(_floor.sizeX/2), 0, _floor.sizeZ/2), Quaternion.identity); 
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.eulerAngles = new Vector3(0, 90, 0);
            }

            // 오른쪽에 다른 노드가 있는가? 
            if (!nodeHash.Contains(new Vector2(_nodes[i].x + 1, _nodes[i].z)))
            {
                var go = Instantiate(railing.prefab, curNode + new Vector3(_floor.sizeX/2, 0, _floor.sizeZ/2), Quaternion.identity); 
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.eulerAngles = new Vector3(0, -90, 0);
            }

            // 위에 다른 노드가 있는가?
            if (!nodeHash.Contains(new Vector2(_nodes[i].x, _nodes[i].z + 1)))
            {
                var go = Instantiate(railing.prefab, curNode + new Vector3(0, 0, _floor.sizeZ), Quaternion.identity); 
                go.transform.localScale = new Vector3(-1, 1, 1);
            }
 
            // 아래에 다른 노드가 있는가?
            if (!nodeHash.Contains(new Vector2(_nodes[i].x, _nodes[i].z - 1)))
            {
                Instantiate(railing.prefab, curNode, Quaternion.identity); 
            }
        }

        // 난간 설치 알고리즘
        // DFS를 사용해서 모든 노드에 대해서 난간 설치
        // 1. 해당 노드의 상하좌우에 인접한 다른 노드가 있는지 체크한다
        // 2. 인접한 노드가 없는경우에만 해당 방향에 난간을 설치한다


    }

    private void Union(int[] parents, int a, int b)
    {
        a = Find(parents, a);
        b = Find(parents, b);

        parents[b] = a;
    }
    private int Find(int[] parents, int a)
    {
        if (parents[a] == a)
            return a;

        return parents[a] = Find(parents, parents[a]);
    }
}
