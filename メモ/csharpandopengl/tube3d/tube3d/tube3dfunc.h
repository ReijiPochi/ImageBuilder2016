#ifndef __TUBE3DFUNC__
#define __TUBE3DFUNC__

#define ERRORCODE_TUBE3D_NON					0
#define ERRORCODE_TUBE3D_DOUBLECREATE			-1
#define ERRORCODE_TUBE3D_NOTCREATE				-2

#ifdef __cplusplus
  extern "C"
  {
#endif
//メソッド
int WINAPI TUBE3D_Create();										//インスタンス生成
int WINAPI TUBE3D_Destroy();									//インスタンス破棄

int WINAPI TUBE3D_Open();										//オープン
int WINAPI TUBE3D_Close();										//クローズ


int WINAPI TUBE3D_DrawData();				

int WINAPI TUBE3D_SetMousePosUp(int x, int y);				
int WINAPI TUBE3D_SetMousePosDown(int x, int y);				
int WINAPI TUBE3D_SetMousePosMove(int x, int y);				


int WINAPI TUBE3D_set_WindowHandle(HANDLE h);					//ウインドウハンドルのセット
#ifdef __cplusplus
  }
#endif

#endif

