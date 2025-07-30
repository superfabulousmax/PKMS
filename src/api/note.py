from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List
from src.schemas.note import NoteCreate, NoteRead
from src.controller.note import \
    create_note, get_notes, get_note, update_note, delete_note
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