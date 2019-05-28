using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelsFactory{

    protected static List<LevelsFactory> levelList = new List<LevelsFactory>();
    public static List<LevelsFactory> LevelsTest(){
        return new List<LevelsFactory>(levelList);
    }

    public abstract string GetDescription();
    public abstract string GetObjective();
}

public class LevelOne: LevelsFactory{
    private string description, objective;

    public LevelOne(string newDescription, string newObjective){
         description = newDescription;
         objective = newObjective;
    }

    public override string GetDescription(){ 
        return description; 
    }
    public override string GetObjective(){
        return objective;
    }
}

public class LevelTwo: LevelsFactory{
    private string description, objective;

    public LevelTwo(string newDescription, string newObjective){
         description = newDescription;
         objective = newObjective;
    }

    public override string GetDescription(){ 
        return description; 
    }
    public override string GetObjective(){
        return objective;
    }
}

public class LevelsTest : MonoBehaviour{

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == ("Player")) {
            LevelOne levelOne = new LevelOne(this.gameObject.name, "Hello1");
            LevelTwo levelTwo = new LevelTwo("Hello2", "Hello2");
            LevelsFactory[] levels = new LevelsFactory[] {levelOne, levelTwo};
            foreach(LevelsFactory factory in levels){
                Debug.Log(factory.GetDescription());
            }
        }
    }
}
