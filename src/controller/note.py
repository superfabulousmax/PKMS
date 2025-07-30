from sqlalchemy.future import select
from src.models.note import Note
from src.schemas.note import NoteCreate
from sqlalchemy.ext.asyncio import AsyncSession


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