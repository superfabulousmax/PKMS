from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from sqlalchemy.ext.asyncio import AsyncSession
from typing import List
from src.schemas.note import NoteCreate, NoteRead
from src.controller.note import \
    create_note, get_notes, get_note, update_note, delete_note, link_notes, search_notes
from src.db.session import get_db


router = APIRouter()


@router.post("/", response_model=NoteRead)
async def create(note: NoteCreate, db: Session = Depends(get_db)):
    return await create_note(db, note)


@router.get("/", response_model=List[NoteRead])
async def get_all(db: Session = Depends(get_db)):
    return await get_notes(db)


@router.get("/{note_id}", response_model=NoteRead)
async def note_get(note_id: int, db: Session = Depends(get_db)):
    db_note = await get_note(db, note_id)
    if not db_note:
        raise HTTPException(status_code=404, detail="Note not found")
    return db_note


@router.put("/{note_id}", response_model=NoteRead)
async def note_update(note_id: int, note_update: NoteCreate,
                      db: Session = Depends(get_db)):
    db_note = await update_note(db, note_id, note_update)

    if not db_note:
        raise HTTPException(status_code=404, detail="Note not found")
    return db_note


@router.delete("/{note_id}")
async def note_delete(note_id: int, db: Session = Depends(get_db)):
    db_note = await delete_note(db, note_id)
    if not db_note:
        raise HTTPException(status_code=404, detail="Note not found")
    return {"detail": "Note deleted"}


@router.post("/notes/{from_note_id}/link/{to_note_id}")
async def link_note_endpoint(
    from_note_id: int,
    to_note_id: int,
    db: AsyncSession = Depends(get_db),
):
    note = await link_notes(db, from_note_id, to_note_id)
    if not note:
        raise HTTPException(
            status_code=404,
            detail="One or both notes not found")
    return note

@router.get("/search/{query}")
async def search_note_endpoint(
    query: str,
    db: AsyncSession = Depends(get_db),
):
    note = await search_notes(db, query)
    return note