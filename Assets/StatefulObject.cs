using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Used to specify a default state to be used upon entry into the state machine
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class DefaultStateAttribute : Attribute
{

}

public interface IState 
{
    /// <summary>
    /// Called one time upon the first time entering the state. Useful for setting up listeners or acquiring references.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">A reference to the controller object</param>
    public void OnStateStart<T>(StatefulObject<T> self) where T : IState;
    /// <summary>
    /// Called once upon entry every time this state is entered.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">A reference to the controller object</param>
    public void OnStateEnter<T>(StatefulObject<T> self) where T : IState;
    /// <summary>
    /// Called once every frame. Identical to Unity Update.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">A reference to the controller object</param>
    public void OnStateUpdate<T>(StatefulObject<T> self) where T : IState;
    /// <summary>
    /// Called at a fixed framerate. Identical to Unity FixedUpdate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">A reference to the controller object</param>
    public void OnStateFixedUpdate<T>(StatefulObject<T> self) where T : IState;
    /// <summary>
    /// Called once every time this state is exited
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">A reference to the controller object</param>
    public void OnStateExit<T>(StatefulObject<T> self) where T : IState;
}

public class State<T> : OmniEnum<State<T>, T> where T : IState
{
    internal bool firstTime = true;

    public static implicit operator State<T>(T data)
    {
        var x = new State<T>();
        x.data = data;
        return x;
    }
    public void OverrideState(T newData)
    {
        data = newData;
    }
}

public class StatefulObject<T> : MonoBehaviour where T : IState
{
    public static StatefulObject<T> Instance;

    protected State<T> state;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //Getting the fields of the type that inherits from us.
        //Because each generic definition of a State is treated as a different type, there should only be one class that inherits from the State hence index 0
        var fieldList = Assembly.GetAssembly(typeof(State<T>)).GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(State<T>))).ToList()[0].GetFields();
        var stateDefault = new List<FieldInfo>(fieldList).Find(field => Attribute.IsDefined(field, typeof(DefaultStateAttribute)));
        if (stateDefault != null)   //check to make sure the states we check had a default attribute
        {
            state = (State<T>)stateDefault.GetValue(null);
            state.data.OnStateEnter(this);
        }
    }

    protected virtual void Update()
    {
        state.data.OnStateUpdate(this);
    }

    protected virtual void FixedUpdate()
    {
        state.data.OnStateFixedUpdate(this);
    }

    public void ChangeState(State<T> newState)
    {
        //call the exit on the current state
        state.data.OnStateExit(this);

        //change the state
        state = newState;

        if (state.firstTime)
        {
            state.data.OnStateStart(this);
            state.firstTime = false;
        }
            

        //call enter on the new state in IStates
        newState.data.OnStateEnter(this);
    }
}
