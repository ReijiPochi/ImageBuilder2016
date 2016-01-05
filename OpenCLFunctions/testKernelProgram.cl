__kernel void myKernelFunction(__global float* numbers)
{
	int globalThreadID = get_global_id(0);
    numbers[globalThreadID] = globalThreadID;
}
