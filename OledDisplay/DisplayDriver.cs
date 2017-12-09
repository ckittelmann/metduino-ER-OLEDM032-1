using Microsoft.SPOT.Hardware;
using System.Threading;
using SecretLabs.NETMF.Hardware.Netduino;

namespace OledDisplay
{
    public abstract class DisplayDriver
    {
        private static SPI.Configuration spiConfig = new SPI.Configuration(
				ChipSelect_Port: Pins.GPIO_PIN_D10,      // Chip select is digital IO 10.
				ChipSelect_ActiveState: false,          // Chip select is active low.
				ChipSelect_SetupTime: 0,                // Amount of time between selection and the clock starting
				ChipSelect_HoldTime: 0,                 // Amount of time the device must be active after the data has been read.
				Clock_Edge: true,                      // Sample on the falling edge.
				Clock_IdleState: true,                  // Clock is idle when high.
				Clock_RateKHz: 5000,                    // 5MHz clock speed.
				SPI_mod: SPI_Devices.SPI1               // Use SPI1
			);
        private SPI spi = new SPI(spiConfig);
        private OutputPort reset = new OutputPort(Pins.GPIO_PIN_D0, true);
        private OutputPort commandData = new OutputPort(Pins.GPIO_PIN_D1, false);
        protected byte[] displayData = new byte[256*64/2]; // every byte represents two pixels

        public void Start()
        {
            InitDisplay();

            SetRowAddress(0);
            SetColumnAddress(0);
            WriteInstruction(0x5c);
            
            StartAnimation();
        }

        protected abstract void StartAnimation();

        protected void RenderDisplayData()
        {
            WriteData(displayData);
        }

        private void InitDisplay()
        {
            ResetDisplay();

            WriteInstruction(0xFD);
            WriteInstruction(0xFD);
            WriteData(0x12); /* UNLOCK */
            WriteInstruction(0xAE); /*DISPLAY OFF*/
            WriteInstruction(0xB3);/*DISPLAYDIVIDE CLOCKRADIO/OSCILLATAR FREQUANCY*/
            WriteData(0x91);
            WriteInstruction(0xCA); /*multiplex ratio*/
            WriteData(0x3F); /*duty = 1/64*/
            WriteInstruction(0xA2); /*set offset*/
            WriteData(0x00);
            WriteInstruction(0xA1); /*start line*/
            WriteData(0x00);
            WriteInstruction(0xA0); /*set remap*/
            WriteData(0x14);
            WriteData(0x11);

            /*Write_Instruction(0xB5); //GPIO Write_Instruction(0x00); */
            WriteInstruction(0xAB); /*funtion selection*/
            WriteData(0x01); /* selection external vdd */
            WriteInstruction(0xB4); /* */
            WriteData(0xA0);
            WriteData(0xfd);
            WriteInstruction(0xC1); /*set contrast current */
            WriteData(0x80);
            WriteInstruction(0xC7); /*master contrast current control*/
            WriteData(0x0f);

            /*Write_Instruction(0xB9); GRAY TABLE*/
            WriteInstruction(0xB1); /*SET PHASE LENGTH*/
            WriteData(0xE2);
            WriteInstruction(0xD1); /**/
            WriteData(0x82);
            WriteData(0x20);
            WriteInstruction(0xBB); /*SET PRE-CHANGE VOLTAGE*/
            WriteData(0x1F);
            WriteInstruction(0xB6); /*SET SECOND PRE-CHARGE PERIOD*/
            WriteData(0x08);
            WriteInstruction(0xBE); /* SET VCOMH */
            WriteData(0x07);
            WriteInstruction(0xA6); /*normal display*/
            ClearRam();
            WriteInstruction(0xAF); /*display ON*/

            //WriteInstruction(0xa5);//--all display on
            //Thread.Sleep(500);

            //WriteInstruction(0xa4);//--all Display off
            //Thread.Sleep(500);

            WriteInstruction(0xa6);//--set normal display
        }

        private void WriteInstruction(byte value)
        {
            commandData.Write(false);
            spi.Write(new byte[] { value });
        }

        private void WriteData(byte value)
        {
            WriteData(new byte[] { value });
        }

        private void WriteData(byte[] value)
        {
            commandData.Write(true);
            spi.Write(value);
        }

        private void ClearRam()
        {
            WriteInstruction(0x15);
            WriteData(0x00);
            WriteData(0x77);
            WriteInstruction(0x75);
            WriteData(0x00);
            WriteData(0x7f);
            WriteInstruction(0x5C);

            byte[] bytesToWrite = new byte[128*120];
            for (int y = 0; y < 128*120; y++)
            {
                bytesToWrite[y] = 0x00;
            }

            WriteData(bytesToWrite);
        }

        private void ResetDisplay()
        {
            reset.Write(false);
            Thread.Sleep(100);
            reset.Write(true);
            Thread.Sleep(100);
        }

        // Set row address 0~32
        private void SetRowAddress(byte add)
        {
            WriteInstruction(0x75);/*SET SECOND PRE-CHARGE PERIOD*/
            add = (byte)(0x3f & add);
            WriteData(add);
            WriteData(0x3f);
            return;
        }

        // Set row address 0~64  for Gray mode)
        private void SetColumnAddress(byte add)
        {
            add = (byte)(0x3f & add);
            WriteInstruction(0x15); /*SET SECOND PRE-CHARGE PERIOD*/
            WriteData((byte)(0x1c + add));
            WriteData(0x5b);
        }
    }
}
