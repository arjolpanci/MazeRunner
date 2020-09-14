using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthGenerator : MonoBehaviour
{

    public GameObject wall, player, light, finish;
    public int labSize;
    public Segment[,] labSegments;

    private int currentX=0, currentY=0, maxSize;
    private bool isFinished=false;

    // Start is called before the first frame update
    void Start()
    {
        maxSize = (labSize-1)*6;
        InitializeGrid(labSize);
        GenerateMaze();
        player.transform.position = new Vector3(0, 1.2F, 0);
        Instantiate(finish, new Vector3(maxSize, 2.2F, maxSize), Quaternion.identity);
    }

    public void GenerateMaze(){

        labSegments[currentX, currentY].visited=true;
        labSegments[currentX, currentY].floor.name = "Visited";

        while(!isFinished){
            digPath();
            reset();
        }

    }

    public bool pathAvailable(int x, int y){
        int totalPaths = 0;

        if (x>0 && !labSegments[x-6, y].visited) totalPaths++;
        if (x<maxSize && !labSegments[x+6, y].visited) totalPaths++;
        if (y>0 && !labSegments[x, y-6].visited) totalPaths++;
        if (y<maxSize && !labSegments[x, y+6].visited) totalPaths++;

        return totalPaths > 0;
    }

    public bool segmentWithinBounds(int x, int y){
        if(x>=0 && x<=maxSize && y>=0 && y<=maxSize && !labSegments[x, y].visited) return true;
        return false;
    }

    public void digPath(){

        System.Random rnd = new System.Random();
        int nextPath = 0;

        while(pathAvailable(currentX, currentY)){
            nextPath = rnd.Next(1,5);

            if(nextPath == 1 && segmentWithinBounds(currentX+6, currentY)){
                //North
                if(labSegments[currentX+6, currentY].southWall != null){
                    Destroy(labSegments[currentX+6, currentY].southWall);
                }
                currentX+=6;
            }else if(nextPath == 2 && segmentWithinBounds(currentX, currentY-6)){
                //Right
                if(labSegments[currentX, currentY-6].leftWall != null){
                    Destroy(labSegments[currentX, currentY-6].leftWall);
                }
                currentY-=6;
            }else if(nextPath == 3 && segmentWithinBounds(currentX-6, currentY)){
                //South
                if(labSegments[currentX, currentY].southWall != null){
                    Destroy(labSegments[currentX, currentY].southWall);
                }
                currentX-=6;
            }else if(nextPath == 4 && segmentWithinBounds(currentX, currentY+6)){
                //Left
                if(labSegments[currentX, currentY].leftWall != null){
                    Destroy(labSegments[currentX, currentY].leftWall);
                }
                currentY+=6;
            }
            labSegments[currentX, currentY].visited = true;
            labSegments[currentX, currentY].floor.name = "Visited";
        }

    }

    public bool isPathPresent(int x, int y){
        
        int availablePaths = 0;

        if (x>0 && labSegments[x-6, y].visited) availablePaths++;
        if (x<maxSize && labSegments[x+6, y].visited) availablePaths++;
        if (y>0 && labSegments[x, y-6].visited) availablePaths++;
        if (y<maxSize && labSegments[x, y+6].visited) availablePaths++;

        return availablePaths > 0;

    }

    public bool isValidPath(int x, int y){
        if (x>=0 && x<=maxSize && y>=0 && y<=maxSize) return true;
        return false;
    }

    public void openRandomPath(int x, int y){

        System.Random rnd = new System.Random();
        int nextPath = 0;
        bool pathOpened = false;

        while(!pathOpened){
            nextPath = rnd.Next(1,5);

            if(nextPath == 1 && isValidPath(currentX+6, currentY)){
                //North
                if(labSegments[currentX+6, currentY].southWall != null && labSegments[currentX+6, currentY].visited){
                    Destroy(labSegments[currentX+6, currentY].southWall);
                    pathOpened = true;
                }
            }else if(nextPath == 2 && isValidPath(currentX, currentY-6)){
                //Right
                if(labSegments[currentX, currentY-6].leftWall != null && labSegments[currentX, currentY-6].visited){
                    Destroy(labSegments[currentX, currentY-6].leftWall);
                    pathOpened = true;
                }
            }else if(nextPath == 3 && isValidPath(currentX-6, currentY)){
                //South
                if(labSegments[currentX, currentY].southWall != null && labSegments[currentX-6, currentY].visited){
                    Destroy(labSegments[currentX, currentY].southWall);
                    pathOpened = true;
                }
            }else if(nextPath == 4 && isValidPath(currentX, currentY+6)){
                //Left
                if(labSegments[currentX, currentY].leftWall != null && labSegments[currentX, currentY+6].visited){
                    Destroy(labSegments[currentX, currentY].leftWall);
                    pathOpened = true;
                }
            }
        }

    }

    public void reset(){
        isFinished = true;

        for(int i=0; i<6*labSize; i+=6){
            for(int j=0; j<6*labSize; j+=6){
                if(!labSegments[i, j].visited && isPathPresent(i, j)){
                    isFinished = false;
                    currentX = i;
                    currentY = j;
                    openRandomPath(currentX, currentY);
                    labSegments[currentX, currentY].visited = true;
                    labSegments[currentX, currentY].floor.name = "Visited";
                    return;
                }
            }
        }

    }

    public void InitializeGrid(int size){
        labSegments = new Segment[size*6, size*6];

        for(int x=0; x<6*size; x+=6){
            for(int y=0; y<6*size; y+=6){
                labSegments[x,y] = new Segment();
                
                //Generating Floor
                labSegments[x,y].floor = Instantiate(wall, new Vector3(x,0,y), Quaternion.identity);
                labSegments[x,y].floor.transform.Rotate(new Vector3(90F, 0, 0));

                //Generating Ceiling
                //labSegments[x,y].ceiling = Instantiate(wall, new Vector3(x,6,y), Quaternion.identity);
                //labSegments[x,y].ceiling.transform.Rotate(new Vector3(90F, 0, 0));

                //Light
                labSegments[x,y].light = Instantiate(light, new Vector3(x,5,y), Quaternion.identity);
                
                //Generating Right Walls
                if(y==0){
                    labSegments[x,y].rightWall = Instantiate(wall, new Vector3(x,3,y-3), Quaternion.identity);
                    labSegments[x,y].rightWall.name = "RightWall"; 
                }

                //Generating Left Walls
                labSegments[x,y].leftWall = Instantiate(wall, new Vector3(x,3,y+3), Quaternion.identity);
                labSegments[x,y].leftWall.name = "LeftWall";   

                //Generating South Walls
                labSegments[x,y].southWall = Instantiate(wall, new Vector3(x-3,3,y), Quaternion.identity);
                labSegments[x,y].southWall.transform.Rotate(new Vector3(0, 90F, 0));
                labSegments[x,y].southWall.name = "SouthWall";

                //Generating North Walls
                if(x==maxSize){
                    labSegments[x,y].northWall = Instantiate(wall, new Vector3(x+3,3,y), Quaternion.identity);
                    labSegments[x,y].northWall.transform.Rotate(new Vector3(0, 90F, 0));
                    labSegments[x,y].northWall.name = "NorthWall";
                }
                
            }
        }
    }

    public void resetLevel(){
        isFinished=false;
        foreach(Segment s in labSegments){
            if(s != null){
                if(s.light != null) Destroy(s.light);
                if(s.ceiling != null) Destroy(s.ceiling);
                if(s.floor != null) Destroy(s.floor);
                if(s.northWall != null) Destroy(s.northWall);
                if(s.rightWall != null) Destroy(s.rightWall);
                if(s.southWall != null) Destroy(s.southWall);
                if(s.leftWall != null) Destroy(s.leftWall); 
            } 
        }
        InitializeGrid(labSize);
        GenerateMaze();
        finish.transform.position = new Vector3(maxSize, 2.2F, maxSize);
    }
    
    void Update()
    {
    }
}
