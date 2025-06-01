import { Theme } from '@emotion/react'
import { SxProps } from '@mui/material'

export const overflowEllipsisStyle: SxProps<Theme> = {
  overflow: 'hidden',
  textOverflow: 'ellipsis',
  whiteSpace: 'nowrap',
  maxWidth: 1
} as const

export const mainContainerStyle: SxProps<Theme> = {
  height: 'calc(100vh - 64px)',
  overflow: 'scroll'
} as const

export const flexCenterStyle: SxProps<Theme> = {
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
  minHeight: 1
} as const
