#pragma once

namespace Tomato
{
	union TOMATO_API TimerCounter
	{
		LARGE_INTEGER HighRes;
		DWORD LowRes;
	};

	class TOMATO_API Timer 
	{
	public:
		Timer() ;
		~Timer() ;

	public:
		f64 GetFrequency();

		bool IsHighResolution()  { return m_bHighResolution; }

		TimerCounter GetCurrentCounter();

		f64 GetElapsedTime();

		f64 GetTotalElapsedTime();

	private:
		Timer( const Timer& copy );
		Timer& operator = ( const Timer& copy );

	private:
		bool m_bHighResolution;

		TimerCounter m_lastValue;
		TimerCounter m_startingValue;

		f64 m_frequency;
	};

}