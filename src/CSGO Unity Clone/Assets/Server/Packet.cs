﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Assets.Server
{
    /// <summary>Sent from server to client.</summary>
    public enum ServerPackets
    {
        Welcome = 1,
        SpawnPlayer,
        PlayerPosition,
        PlayerRotation,
        PlayerDisconnected,
        PlayerHealth,
        PlayerRespawned
    }

    /// <summary>Sent from client to server.</summary>
    public enum ClientPackets
    {
        WelcomeReceived = 1,
        PlayerMovement,
        PlayerShoot
    }

    public class Packet : IDisposable
    {
        private List<byte> Buffer;
        private byte[] ReadableBuffer;
        private int ReadPos;

        /// <summary>Creates a new empty packet (without an ID).</summary>
        public Packet()
        {
            Buffer = new List<byte>(); // Intitialize buffer
            ReadPos = 0; // Set readPos to 0
        }

        /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="id">The packet ID.</param>
        public Packet(int id)
        {
            Buffer = new List<byte>(); // Intitialize buffer
            ReadPos = 0; // Set readPos to 0

            Write(id); // Write packet id to the buffer
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
        /// <param name="data">The bytes to add to the packet.</param>
        public Packet(byte[] data)
        {
            Buffer = new List<byte>(); // Intitialize buffer
            ReadPos = 0; // Set readPos to 0

            SetBytes(data);
        }

        #region Functions
        /// <summary>Sets the packet's content and prepares it to be read.</summary>
        /// <param name="data">The bytes to add to the packet.</param>
        public void SetBytes(byte[] data)
        {
            Write(data);
            ReadableBuffer = Buffer.ToArray();
        }

        /// <summary>Inserts the length of the packet's content at the start of the buffer.</summary>
        public void WriteLength()
        {
            Buffer.InsertRange(0, BitConverter.GetBytes(Buffer.Count)); // Insert the byte length of the packet at the very beginning
        }

        /// <summary>Inserts the given int at the start of the buffer.</summary>
        /// <param name="value">The int to insert.</param>
        public void InsertInt(int value)
        {
            Buffer.InsertRange(0, BitConverter.GetBytes(value)); // Insert the int at the start of the buffer
        }

        /// <summary>Gets the packet's content in array form.</summary>
        public byte[] ToArray()
        {
            ReadableBuffer = Buffer.ToArray();
            return ReadableBuffer;
        }

        /// <summary>Gets the length of the packet's content.</summary>
        public int Length()
        {
            return Buffer.Count; // Return the length of buffer
        }

        /// <summary>Gets the length of the unread data contained in the packet.</summary>
        public int UnreadLength()
        {
            return Length() - ReadPos; // Return the remaining length (unread)
        }

        /// <summary>Resets the packet instance to allow it to be reused.</summary>
        /// <param name="shouldReset">Whether or not to reset the packet.</param>
        public void Reset(bool shouldReset = true)
        {
            if (shouldReset)
            {
                Buffer.Clear(); // Clear buffer
                ReadableBuffer = null;
                ReadPos = 0; // Reset readPos
            }
            else
            {
                ReadPos -= 4; // "Unread" the last read int
            }
        }
        #endregion

        #region Write Data
        /// <summary>Adds a byte to the packet.</summary>
        /// <param name="value">The byte to add.</param>
        public void Write(byte value)
        {
            Buffer.Add(value);
        }
        /// <summary>Adds an array of bytes to the packet.</summary>
        /// <param name="value">The byte array to add.</param>
        public void Write(byte[] value)
        {
            Buffer.AddRange(value);
        }
        /// <summary>Adds a short to the packet.</summary>
        /// <param name="value">The short to add.</param>
        public void Write(short value)
        {
            Buffer.AddRange(BitConverter.GetBytes(value));
        }
        /// <summary>Adds an int to the packet.</summary>
        /// <param name="value">The int to add.</param>
        public void Write(int value)
        {
            Buffer.AddRange(BitConverter.GetBytes(value));
        }
        /// <summary>Adds a long to the packet.</summary>
        /// <param name="value">The long to add.</param>
        public void Write(long value)
        {
            Buffer.AddRange(BitConverter.GetBytes(value));
        }
        /// <summary>Adds a float to the packet.</summary>
        /// <param name="value">The float to add.</param>
        public void Write(float value)
        {
            Buffer.AddRange(BitConverter.GetBytes(value));
        }
        /// <summary>Adds a bool to the packet.</summary>
        /// <param name="value">The bool to add.</param>
        public void Write(bool value)
        {
            Buffer.AddRange(BitConverter.GetBytes(value));
        }
        /// <summary>Adds a string to the packet.</summary>
        /// <param name="value">The string to add.</param>
        public void Write(string value)
        {
            Write(value.Length); // Add the length of the string to the packet
            Buffer.AddRange(Encoding.ASCII.GetBytes(value)); // Add the string itself
        }
        /// <summary>Adds a Vector3 to the packet.</summary>
        /// <param name="value">The Vector3 to add.</param>
        public void Write(Vector3 value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
        }
        /// <summary>Adds a Quaternion to the packet.</summary>
        /// <param name="value">The Quaternion to add.</param>
        public void Write(Quaternion value)
        {
            Write(value.x);
            Write(value.y);
            Write(value.z);
            Write(value.w);
        }
        #endregion

        #region Read Data
        /// <summary>Reads a byte from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte ReadByte(bool moveReadPos = true)
        {
            if (Buffer.Count > ReadPos)
            {
                // If there are unread bytes
                byte value = ReadableBuffer[ReadPos]; // Get the byte at readPos' position
                if (moveReadPos)
                {
                    // If moveReadPos is true
                    ReadPos += 1; // Increase readPos by 1
                }
                return value; // Return the byte
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        /// <summary>Reads an array of bytes from the packet.</summary>
        /// <param name="length">The length of the byte array.</param>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte[] ReadBytes(int length, bool moveReadPos = true)
        {
            if (Buffer.Count > ReadPos)
            {
                // If there are unread bytes
                byte[] value = Buffer.GetRange(ReadPos, length).ToArray(); // Get the bytes at readPos' position with a range of length
                if (moveReadPos)
                {
                    // If moveReadPos is true
                    ReadPos += length; // Increase readPos by length
                }
                return value; // Return the bytes
            }
            else
            {
                throw new Exception("Could not read value of type 'byte[]'!");
            }
        }

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public short ReadShort(bool moveReadPos = true)
        {
            if (Buffer.Count > ReadPos)
            {
                // If there are unread bytes
                short value = BitConverter.ToInt16(ReadableBuffer, ReadPos); // Convert the bytes to a short
                if (moveReadPos)
                {
                    // If moveReadPos is true and there are unread bytes
                    ReadPos += 2; // Increase readPos by 2
                }
                return value; // Return the short
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        /// <summary>Reads an int from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public int ReadInt(bool moveReadPos = true)
        {
            if (Buffer.Count > ReadPos)
            {
                // If there are unread bytes
                int value = BitConverter.ToInt32(ReadableBuffer, ReadPos); // Convert the bytes to an int
                if (moveReadPos)
                {
                    // If moveReadPos is true
                    ReadPos += 4; // Increase readPos by 4
                }
                return value; // Return the int
            }
            else
            {
                throw new Exception("Could not read value of type 'int'!");
            }
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong(bool moveReadPos = true)
        {
            if (Buffer.Count > ReadPos)
            {
                // If there are unread bytes
                long value = BitConverter.ToInt64(ReadableBuffer, ReadPos); // Convert the bytes to a long
                if (moveReadPos)
                {
                    // If moveReadPos is true
                    ReadPos += 8; // Increase readPos by 8
                }
                return value; // Return the long
            }
            else
            {
                throw new Exception("Could not read value of type 'long'!");
            }
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat(bool moveReadPos = true)
        {
            if (Buffer.Count > ReadPos)
            {
                // If there are unread bytes
                float value = BitConverter.ToSingle(ReadableBuffer, ReadPos); // Convert the bytes to a float
                if (moveReadPos)
                {
                    // If moveReadPos is true
                    ReadPos += 4; // Increase readPos by 4
                }
                return value; // Return the float
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool(bool moveReadPos = true)
        {
            if (Buffer.Count > ReadPos)
            {
                // If there are unread bytes
                bool value = BitConverter.ToBoolean(ReadableBuffer, ReadPos); // Convert the bytes to a bool
                if (moveReadPos)
                {
                    // If moveReadPos is true
                    ReadPos += 1; // Increase readPos by 1
                }
                return value; // Return the bool
            }
            else
            {
                throw new Exception("Could not read value of type 'bool'!");
            }
        }

        /// <summary>Reads a string from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString(bool moveReadPos = true)
        {
            try
            {
                int length = ReadInt(); // Get the length of the string
                string value = Encoding.ASCII.GetString(ReadableBuffer, ReadPos, length); // Convert the bytes to a string
                if (moveReadPos && value.Length > 0)
                {
                    // If moveReadPos is true string is not empty
                    ReadPos += length; // Increase readPos by the length of the string
                }
                return value; // Return the string
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }

        /// <summary>Reads a Vector3 from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Vector3 ReadVector3(bool moveReadPos = true)
        {
            return new Vector3(ReadFloat(moveReadPos), ReadFloat(moveReadPos), ReadFloat(moveReadPos));
        }

        /// <summary>Reads a Quaternion from the packet.</summary>
        /// <param name="moveReadPos">Whether or not to move the buffer's read position.</param>
        public Quaternion ReadQuaternion(bool moveReadPos = true)
        {
            return new Quaternion(ReadFloat(moveReadPos), ReadFloat(moveReadPos), ReadFloat(moveReadPos), ReadFloat(moveReadPos));
        }
        #endregion

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Buffer = null;
                    ReadableBuffer = null;
                    ReadPos = 0;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}