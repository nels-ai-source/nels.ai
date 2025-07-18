import { useState, useRef, useEffect, useCallback } from 'react'

interface ScrollState {
  userScrolled: boolean;
  disableAutoScroll: boolean;
}

interface ScrollElement extends HTMLElement {
  scrollHeight: number;
  scrollTop: number;
  clientHeight: number;
}

export const useScroll = (loading: boolean) => {
  const [scrollState, setScrollState] = useState<ScrollState>({
    userScrolled: false,
    disableAutoScroll: false,
  })
  const scrollContainer = useRef<ScrollElement | null>(null)

  const handleScroll = useCallback((e: Event) => {
    if (!scrollState.userScrolled || !loading) return
    const target = e.target as ScrollElement
    const scrollHeight = target.scrollHeight
    const height = target.scrollTop + target.clientHeight
    const diffHeight = scrollHeight - height
    setScrollState(prev => ({
      ...prev,
      disableAutoScroll: diffHeight >= 10,
      userScrolled: diffHeight >= 10
    }))
  }, [scrollState.userScrolled, loading])

  const onWheel = useCallback(() => {
    if (scrollState.userScrolled || !loading) return
    setScrollState(prev => ({
      ...prev,
      userScrolled: true
    }))
  }, [scrollState.userScrolled, loading])

  const onListenScroll = useCallback((tableInnerRef: ScrollElement) => {
    scrollContainer.current = tableInnerRef
    tableInnerRef.addEventListener('scroll', handleScroll)
    tableInnerRef.addEventListener('wheel', onWheel)
  }, [handleScroll, onWheel])

  const scrollToBottom = useCallback(() => {
    if (scrollState.disableAutoScroll) return
    if (!scrollContainer.current) return
    scrollContainer.current.scrollTop = scrollContainer.current.scrollHeight + 20
  }, [scrollState.disableAutoScroll])

  useEffect(() => {
    return () => {
      if (!scrollContainer.current) return
      scrollContainer.current.removeEventListener('scroll', handleScroll)
      scrollContainer.current.removeEventListener('wheel', onWheel)
    }
  }, [handleScroll, onWheel])

  return { onListenScroll, scrollToBottom }
}
