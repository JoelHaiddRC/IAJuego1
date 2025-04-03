using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

// Notes
// Implementacion de Maquinas de Estado tomada de Jason Weimman

public class StateMachine
{
   private Estado _estadoActual;

   private Dictionary<Type, List<Transicion>> _transiciones = new Dictionary<Type, List<Transicion>>();
   private List<Transicion> _transicionesActuales =  new List<Transicion>();
   /*
   private IState _currentState;
   
   private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
   private List<Transition> _currentTransitions = new List<Transition>();
   */
   private List<Transicion> _anyTransitions = new List<Transicion>();
   
   private static List<Transicion> EmptyTransitions = new List<Transicion>(0);

   public void Tick()
   {
      var transicion = ObtenerTransicion();
      if (transicion != null)
         AsignarEstado(transicion.Destino);
      
      _estadoActual?.Tick();
      /*
      var transition = GetTransition();
      if (transition != null)
         SetState(transition.To);
      
      _currentState?.Tick();*/
   }

   public void AsignarEstado(Estado estado)//SetState(IState state)
   {
      if (estado == _estadoActual) 
         return;
      _estadoActual?.OnExit();
      _estadoActual = estado;

      _transiciones.TryGetValue(_estadoActual.GetType(), out _transicionesActuales);
      if (_transicionesActuales == null)
         _transicionesActuales = EmptyTransitions;
      
      _estadoActual.OnEnter();
      /*
      if (state == _currentState)
         return;
      
      _currentState?.OnExit();
      _currentState = state;
      
      _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
      if (_currentTransitions == null)
         _currentTransitions = EmptyTransitions;
      
      _currentState.OnEnter();
      */
   }

   public void AgregarTransicion(Estado origen, Estado destino, Func<bool> condicion)//AddTransition(IState from, IState to, Func<bool> predicate)
   {
      if (_transiciones.TryGetValue(origen.GetType(), out var transiciones) == false)
      {
         transiciones = new List<Transicion>();
         _transiciones[origen.GetType()] = transiciones;
      }
      transiciones.Add(new Transicion(destino, condicion));
      /*
      if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
      {
         transitions = new List<Transition>();
         _transitions[from.GetType()] = transitions;
      }
      
      transitions.Add(new Transition(to, predicate));
      */
   }

   public void AddAnyTransition(Estado state, Func<bool> predicate)
   {
      _anyTransitions.Add(new Transicion(state, predicate));
   }

   private class Transicion  //Transition
   {
      public Func<bool> Condicion {get; }//Condition {get; }
      public Estado Destino {get; }//IState To { get; }

      public Transicion(Estado destino, Func<bool> condicion)
      {
         Destino = destino;
         Condicion = condicion;
      }
   }

   private Transicion ObtenerTransicion()//Transition GetTransition()
   {
      foreach (var transicion in _transicionesActuales)
         if (transicion.Condicion())
            return transicion;
      
      foreach (var transicion in _anyTransitions)
         if (transicion.Condicion())
            return transicion;

      return null;
      
      /*foreach(var transition in _anyTransitions)
         if (transition.Condition())
            return transition;
      
      foreach (var transition in _currentTransitions)
         if (transition.Condition())
            return transition;

      return null;*/
   }
}