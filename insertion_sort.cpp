#include <iostream>
using namespace std;

void Sort(int* arr,int n)
{
	int counter=0;
	for(int i=1;i<n;i++){
		for(int j=i; j>0 && arr[j-1]>arr[j];j--){
			counter++;
			int tmp=arr[j-1];
			arr[j-1]=arr[j];
			arr[j]=tmp;
		}
	}
	cout<<counter <<endl;
	for (int i = 1; i < n; i++)
	{
	    cout << arr[i];
	}
}
int main()
{
    int numbers[9] {1, 2, 4, 2, 4, 3, 5, 5, 6};
    int n = 9;
    Sort(numbers, n);
};