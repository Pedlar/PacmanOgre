using PacmanOgre.Input;
using SharpEngine;

namespace PacmanOgre.Components
{
    class PlayerInputComponent : IComponent
    {
        private static float SPEED = 5f;

        private readonly IContext _context;
        private readonly IEntity _entity;

        private org.ogre.Vector3 _currentApplyVelocity = new org.ogre.Vector3(0f);
        private VelocityComponent _velocityComponent;

        public PlayerInputComponent(IContext context, IEntity entity)
        {
            _context = context;
            _entity = entity;

            InputManager inputManager = context.GetInputManager();
            inputManager.OnKeyPressed += OnKeyPressed;
            inputManager.OnKeyReleased += OnKeyReleased;

            ITickManager tickManager = context.GetITickManager();
            tickManager.OnFrameQueued += OnFrameQueued;
        }

        public void OnLoaded()
        {

        }

        public void Setup()
        {
            _velocityComponent = _entity.GetComponent<VelocityComponent>();
        }

        #region events
        public void OnKeyPressed(object source, KeyboardEventArgs eventArgs)
        {
            if(eventArgs.KeyboardEvent.repeat == 1)
            {
                return; // Don't worry about processing holding down the key
            }

            switch ((char)eventArgs.KeyboardEvent.keysym.sym)
            {
                case 'P': // Left
                    _velocityComponent.Velocity.Decrement(new org.ogre.Vector3(SPEED, 0f, 0f));
                    break;
                case 'O': // Right
                    _velocityComponent.Velocity.Increment(new org.ogre.Vector3(SPEED, 0f, 0f));
                    break;
                case 'R': // Up
                    _velocityComponent.Velocity.Decrement(new org.ogre.Vector3(0f, 0f, SPEED));
                    break;
                case 'Q': // Down
                    _velocityComponent.Velocity.Increment(new org.ogre.Vector3(0f, 0f, SPEED));
                    break;
            }
        }

        public void OnKeyReleased(object source, KeyboardEventArgs eventArgs)
        {
            switch ((char)eventArgs.KeyboardEvent.keysym.sym)
            {
                case 'P': // Left
                    _velocityComponent.Velocity.Increment(new org.ogre.Vector3(SPEED, 0f, 0f));
                    break;
                case 'O': // Right
                    _velocityComponent.Velocity.Decrement(new org.ogre.Vector3(SPEED, 0f, 0f));
                    break;
                case 'R': // Up
                    _velocityComponent.Velocity.Increment(new org.ogre.Vector3(0f, 0f, SPEED));
                    break;
                case 'Q': // Down
                    _velocityComponent.Velocity.Decrement(new org.ogre.Vector3(0f, 0f, SPEED));
                    break;
            }
        }

        public void OnFrameQueued(object source, TickEventArgs tickEventArgs)
        {

        }
        #endregion

        public void Dispose()
        {
            InputManager inputManager = _context.GetInputManager();
            inputManager.OnKeyPressed -= OnKeyPressed;
            inputManager.OnKeyReleased -= OnKeyReleased;

            ITickManager tickManager = _context.GetITickManager();
            tickManager.OnFrameQueued -= OnFrameQueued;
        }
    }
}
