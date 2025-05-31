import { ViewId } from '@/lib/api/types/apiTypes'
import { atom } from 'jotai'

export const counterViewIdAtom = atom(crypto.randomUUID() as ViewId)
