using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace tube3d.classes
{
    public class classtube3DDLL : IDisposable
    {
		#region DLL-API��`
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_Create();						//�C���X�^���X����
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_Destroy();						//�C���X�^���X�j��
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_Open();						//Open
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_Close();						//Close
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_DrawData();
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_SetMousePosUp(int x, int y);
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_SetMousePosDown(int x, int y);
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_SetMousePosMove(int x, int y);
		[DllImport("tube3d.dll")]
		private static extern int TUBE3D_set_WindowHandle(IntPtr h);
		#endregion 

		private bool				m_Disposed = false;

        public IntPtr WindowHandle { set { TUBE3D_set_WindowHandle(value);}}

		//---------------------------------------------
		//�R���X�g���N�^
		//
		//---------------------------------------------
		public classtube3DDLL()
		{
            int re = TUBE3D_Create();
		}

		//---------------------------------------------
		//�f�X�g���N�^�C���\�[�X�̉��
		//
		//---------------------------------------------
		~classtube3DDLL()	{Dispose(false);} //�}�l�[�W�n���\�[�X�̉�������ŌĂяo��
		public void Dispose(){Dispose(true);} //�}�l�[�W�n���\�[�X�̉���L��ŌĂяo��

		//---------------------------------------------
		//���\�[�X�̉��(�{��)
		//
		//---------------------------------------------
		private void Dispose(bool disposing)
		{
			#region
			if(!m_Disposed)   //���ɉ���ς݂��`�F�b�N
			{
				//�܂��̂Ƃ�
				if(disposing)        //�}�l�[�W�n�̃��\�[�X���
				{
					//��������Ƀ}�l�[�W���\�[�X�̉��������
				}

				//�A���}�l�[�W�n�̃��\�[�X�̉��
				//��������ɃA���}�l�[�W���\�[�X�̉��������

                int re = TUBE3D_Destroy();
			}
			m_Disposed = true;         //����ς݃t���O�𗧂Ă�
			#endregion
		}

		//---------------------------------------------
		//�I�[�v��
		//
		//---------------------------------------------
        public int Open()
        {
            int re = TUBE3D_Open();
            return re;
        }

		//---------------------------------------------
		//�N���[�Y
		//
		//---------------------------------------------
        public int Close()
        {
            int re = TUBE3D_Close();
            return re;
        }


		//---------------------------------------------
		//�`��又��
		//
		//---------------------------------------------
        public int DrawData()
        {
            int re = TUBE3D_DrawData();
            return re;
        }

		//---------------------------------------------
		//�}�E�X���W�Z�b�g�A�b�v��
		//
		//---------------------------------------------
        public int SetMousePosUp(int x, int y)
        {
            int re = TUBE3D_SetMousePosUp(x, y);
            return re;
        }

		//---------------------------------------------
		//�}�E�X���W�Z�b�g�A�b�v��
		//
		//---------------------------------------------
        public int SetMousePosDown(int x, int y)
        {
            int re = TUBE3D_SetMousePosDown(x, y);
            return re;
        }

		//---------------------------------------------
		//�}�E�X���W�Z�b�g�A�b�v��
		//
		//---------------------------------------------
        public int SetMousePosMove(int x, int y)
        {
            int re = TUBE3D_SetMousePosMove(x, y);
            return re;
        }
    }
}
