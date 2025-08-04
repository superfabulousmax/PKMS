from sqlalchemy.future import select
from src.models.note import Note
from src.schemas.note import NoteCreate
from sqlalchemy.ext.asyncio import AsyncSession
from typing import List

async def get_note(db: AsyncSession, note_id: int):
    stmt = select(Note).filter(Note.id == note_id)
    result = await db.execute(stmt)
    note = result.scalars().first()
    return note


async def get_notes(db: AsyncSession):
    result = await db.execute(select(Note))
    notes = result.scalars().all()
    return notes


async def create_note(db: AsyncSession, note: NoteCreate):
    db_note = Note(title=note.title, body=note.body)
    db.add(db_note)
    await db.commit()
    await db.refresh(db_note)
    return db_note


async def update_note(db: AsyncSession, note_id: int, note_update: NoteCreate):
    stmt = select(Note).filter(Note.id == note_id)
    result = await db.execute(stmt)
    note = result.scalars().first()

    if note:
        note.title = note_update.title
        note.body = note_update.body
        await db.commit()
        await db.refresh(note)
    return note


async def delete_note(db: AsyncSession, note_id: int):
    stmt = select(Note).filter(Note.id == note_id)
    result = await db.execute(stmt)
    note = result.scalars().first()
    if note:
        await db.delete(note)
        await db.commit()
    return note


async def link_notes(db: AsyncSession, from_note_id: int, to_note_id: int):
    stmt = select(Note).filter(Note.id.in_([from_note_id, to_note_id]))
    result = await db.execute(stmt)
    notes = result.scalars().all()

    if len(notes) != 2:
        return None

    from_note = next((n for n in notes if n.id == from_note_id), None)
    to_note = next((n for n in notes if n.id == to_note_id), None)

    if to_note not in from_note.links:
        from_note.links.append(to_note)
        await db.commit()
        await db.refresh(from_note)

    return from_note


async def search_notes(db: AsyncSession, query: str) -> List[Note]:
    pattern = f"%{query}%"
    stmt = select(Note).filter(Note.title.like(pattern))
    result = await db.execute(stmt)
    return result.scalars().all()