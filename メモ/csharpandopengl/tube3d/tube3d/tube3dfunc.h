#ifndef __TUBE3DFUNC__
#define __TUBE3DFUNC__

#define ERRORCODE_TUBE3D_NON					0
#define ERRORCODE_TUBE3D_DOUBLECREATE			-1
#define ERRORCODE_TUBE3D_NOTCREATE				-2

#ifdef __cplusplus
  extern "C"
  {
#endif
//���\�b�h
int WINAPI TUBE3D_Create();										//�C���X�^���X����
int WINAPI TUBE3D_Destroy();									//�C���X�^���X�j��

int WINAPI TUBE3D_Open();										//�I�[�v��
int WINAPI TUBE3D_Close();										//�N���[�Y


int WINAPI TUBE3D_DrawData();				

int WINAPI TUBE3D_SetMousePosUp(int x, int y);				
int WINAPI TUBE3D_SetMousePosDown(int x, int y);				
int WINAPI TUBE3D_SetMousePosMove(int x, int y);				


int WINAPI TUBE3D_set_WindowHandle(HANDLE h);					//�E�C���h�E�n���h���̃Z�b�g
#ifdef __cplusplus
  }
#endif

#endif

