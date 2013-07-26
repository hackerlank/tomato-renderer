#include "TomatoPCH.h"

#include "Timer.h"

namespace Tomato
{
	Timer::Timer()  
		: m_bHighResolution( false ) 
		, m_lastValue()
		, m_startingValue()
		, m_frequency( 0 )
	{
		TimerCounter frequencyValue;
		if( ::QueryPerformanceFrequency( &frequencyValue.HighRes ) ) 
		{
			m_bHighResolution = true;
			m_frequency = static_cast<f64>( frequencyValue.HighRes.QuadPart );
		} 
		else 
		{
			m_bHighResolution = false;
			m_frequency = 1000.0;
		}

		m_startingValue = GetCurrentCounter();
		GetElapsedTime();
	}

	Timer::~Timer()  
	{ }

	TimerCounter Timer::GetCurrentCounter()  
	{
		TimerCounter value;

		if( m_bHighResolution ) 
		{
			::QueryPerformanceCounter( &value.HighRes );
		} 
		else 
		{
			value.LowRes = GetTickCount();
		}

		return value;
	}

	f64 Timer::GetElapsedTime()  
	{
		if( m_bHighResolution ) 
		{
			LARGE_INTEGER value;
			::QueryPerformanceCounter( &value );
			
			f64 elapsedCounter = static_cast<f64>( value.QuadPart - m_lastValue.HighRes.QuadPart );
			f64 elapsedTime = elapsedCounter / m_frequency;
			
			m_lastValue.HighRes = value;
			
			return elapsedTime;
		} 
		else 
		{
			DWORD value = ::GetTickCount();
			
			f64 elapsedCounter = static_cast<f64>( value - m_lastValue.LowRes );
			f64 elapsedTime = elapsedCounter / m_frequency;
			
			m_lastValue.LowRes = value;
			
			return elapsedTime;
		}
	}

	f64 Timer::GetTotalElapsedTime()  
	{
		if( m_bHighResolution ) 
		{
			LARGE_INTEGER value;
			::QueryPerformanceCounter( &value );
			
			f64 elapsedCounter = static_cast<f64>( value.QuadPart - m_startingValue.HighRes.QuadPart );
			f64 elapsedTime = elapsedCounter / m_frequency;
			
			return elapsedTime;
		} 
		else 
		{
			DWORD value = ::GetTickCount();
			
			f64 elapsedCounter = static_cast<f64>( value - m_startingValue.LowRes );
			f64 elapsedTime = elapsedCounter / m_frequency;
			
			return elapsedTime;
		}
	}

	f64 Timer::GetFrequency()  
	{
		return m_frequency;
	}
}