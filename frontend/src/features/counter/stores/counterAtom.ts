import { ViewId } from '@/lib/api/types/api'
import { atom } from 'jotai'

export const counterViewIdAtom = atom(crypto.randomUUID() as ViewId)
