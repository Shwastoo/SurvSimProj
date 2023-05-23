using System;
using System.Collections.Generic;
using System.Text;

namespace survSimProject
{
    abstract class Scene:SceneInterface
    {
        protected Game gameRef;
        
        public Scene(Game _game)
        {
            gameRef = _game;
        }
        public virtual void Display()
        { 
        }
        public virtual void ArrowLeft()
        {
        }
        public virtual void ArrowRight()
        {
        }
        public virtual void ArrowUp()
        {
        }
        public virtual void ArrowDown()
        {
        }
        public virtual void Enter()
        {

        }
        public virtual void Space()
        {

        }
        public virtual void Tab()
        {
        }
        public virtual void IKey()
        {

        }

        public virtual void Escape()
        {
        }

    }
}
