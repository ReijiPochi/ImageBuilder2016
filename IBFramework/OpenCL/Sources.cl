
__kernel void myKernelFunction(__global float* numbers)
{
    int globalThreadID = get_global_id(0);
    numbers[globalThreadID] += globalThreadID;
}

__kernel void fillRect(__global uchar* result, __global int* trgSize, __global float* color, __global int* offset, __global int* size)
{
	int y = get_global_id(0);
	if(y < offset[1]) return;

	int x_str = offset[0];

	if(x_str < 0)
		x_str = 0;
	if(x_str > trgSize[0])
		x_str = trgSize[0];


	int x_end = offset[0] + size[0];

	if(x_end < 0)
		x_end = 0;
	if(x_end > trgSize[0])
		x_end = trgSize[0];

	int x, index;
	int yOffset = trgSize[0] * 4 * y;
	float trgAlpha = 1.0 - color[3];

	uchar B = color[0] * color[3] * 255;
	uchar G = color[1] * color[3] * 255;
	uchar R = color[2] * color[3] * 255;

	uchar resultB, resultG, resultR;

	for(x = x_str; x < x_end; x++)
	{
		index = yOffset + x * 4;

		resultB = result[index] * trgAlpha + B;
		resultG = result[index + 1] * trgAlpha + G;
		resultR = result[index + 2] * trgAlpha + R;

		result[index] = resultB;
		result[index + 1] = resultG;
		result[index + 2] = resultR;
		result[index + 3] = 255;
	}
}