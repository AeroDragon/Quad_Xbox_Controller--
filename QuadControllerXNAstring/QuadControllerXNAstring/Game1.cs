using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.IO.Ports;
using System.Text;

namespace QuadControllerXNAstring
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        string charToSend;
        private float rightTriger, leftAnalog_X, rightAanlog_X, rightAnalog_Y;
        byte throttle = 0, yaw = 0, pitch = 0, roll = 0;
        //byte[] commands = new byte[5];
        string commands = "";

        //string to byte array
        String str = "Hello";
        byte[] btarr = ASCIIEncoding.ASCII.GetBytes(str);

        SerialPort _serialPort = new SerialPort();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _serialPort.PortName = "serialPort1";
            _serialPort.BaudRate = 115200;
            _serialPort.Parity = Parity.None;
            //_serialPort.ParityReplace = 63;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.PortName = "COM12";

            // Set the read/write timeouts
            _serialPort.ReadTimeout = -1;
            _serialPort.WriteTimeout = -1;
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            getControllerData(); //Gets values from contorller buttons and thumbsticks
            prepCommands(); //Uses controllers values to calculate and create a suitable byte to send by serial.
            openPort(); // opes the serial port
            sendData(); //sends the data stored in the array 'commands'

            // TODO: Add your update logic here

            base.Update(gameTime);

            
        }

        private void getControllerData()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            rightTriger = GamePad.GetState(PlayerIndex.One).Triggers.Right;//ThumbSticks.Left.Y;
            leftAnalog_X = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            rightAnalog_Y = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
            rightAanlog_X = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;
            
        }

        private void prepCommands()
        {
            throttle = ((byte)(Math.Round(rightTriger * 127)));
            //commands[0] = throttle;
            commands += throttle;

            yaw = ((byte)(Math.Round(leftAnalog_X * 127)));
            //commands[1] = yaw;
            commands += yaw;

            pitch = ((byte)(Math.Round(rightAnalog_Y * 127)));
            //commands[2] = pitch;
            commands += pitch;

            roll = ((byte)(Math.Round(rightAanlog_X * 127)));
            //commands[3] = roll;
            commands += roll;
            commands += '\0';            
            
        }

        private void openPort()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
            //Show message if not opened
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void sendData()
        {
            if (_serialPort.IsOpen)
            {
                //_serialPort.Write(commands, 0, commands.Length);
                //Thread.Sleep(50);
            }
            else
            {
                _serialPort.Open();
            }
        }
    }
}
